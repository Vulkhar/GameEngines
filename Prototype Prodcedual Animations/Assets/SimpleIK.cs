#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// source: https://github.com/ditzel/SimpleIK
/// nur neue Kommentare gemacht
/// </summary>

public class SimpleIK : MonoBehaviour
{

    // Kette von den Knochen die aneinander sind -> Arm,...
    public int ChainLength = 2;

    // Orientierungs Transforms wie die Kette sich beugen soll
    public Transform Target;
    public Transform Pole;


    // Iterationen per Update
    [Header("Solver Parameters")]
    public int Iterations = 10;

    // Distanz wann der Solver stoppt
    public float Delta = 0.001f;

    // St‰rke vom Snapback zur start position
    [Range(0, 1)]
    public float SnapBackStrength = 1f;

    protected float[] BonesLength; // L‰nge Knochen
    protected float CompleteLength; // L‰nge des Gelenks
    protected Transform[] Bones; // Reference auf Child Bones
    protected Vector3[] Positions;
    protected Vector3[] StartDirectionSucc;
    protected Quaternion[] StartRotationBone; // Initial Rotation von jedem Knochen
    protected Quaternion StartRotationTarget; // Initial Rotation von Vieh (?)
    protected Transform Root;


    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    void Init()
    {
        // Initialisier Arrays mithilfe von chainLength
        Bones = new Transform[ChainLength + 1];
        Positions = new Vector3[ChainLength + 1];
        BonesLength = new float[ChainLength];
        StartDirectionSucc = new Vector3[ChainLength + 1];
        StartRotationBone = new Quaternion[ChainLength + 1];

        // Suche Root
        Root = transform; // Das Objekt muss am ende des Gelenks sein -> Fingerspitze
        for (var i = 0; i <= ChainLength; i++)
        {
            if (Root == null)
                throw new UnityException("The chain value is longer than the ancestor chain!");
            Root = Root.parent;
        }

        // Initialisier Target - Wo das Gelenk positioniert wird
        if (Target == null) // Suche dir das Target
        {
            Target = new GameObject(gameObject.name + " Target").transform;
            SetPositionRootSpace(Target, GetPositionRootSpace(transform));
        }
        StartRotationTarget = GetRotationRootSpace(Target);


        // Initialisier Bone Daten 
        var current = transform;
        CompleteLength = 0;
        for (var i = Bones.Length - 1; i >= 0; i--)
        {
            Bones[i] = current;
            StartRotationBone[i] = GetRotationRootSpace(current);

            if (i == Bones.Length - 1)
            {
                // Leaf Bone - Ganz auﬂen - Finger
                StartDirectionSucc[i] = GetPositionRootSpace(Target) - GetPositionRootSpace(current);
            }
            else
            {
                // Mid Bones
                StartDirectionSucc[i] = GetPositionRootSpace(Bones[i + 1]) - GetPositionRootSpace(current);
                BonesLength[i] = StartDirectionSucc[i].magnitude;
                CompleteLength += BonesLength[i];
            }

            current = current.parent;
        }



    }

    // Update is called once per frame
    void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        if (Target == null)
            return;

        if (BonesLength.Length != ChainLength)
            Init();

        //  root
        //  (bone0) (bonelen 0) (bone1) (bonelen 1) (bone2)...
        //   x--------------------x--------------------x---...

        //get position
        for (int i = 0; i < Bones.Length; i++)
            Positions[i] = GetPositionRootSpace(Bones[i]);

        var targetPosition = GetPositionRootSpace(Target);
        var targetRotation = GetRotationRootSpace(Target);

        // Kann der erste das Ziel erreichen?
        if ((targetPosition - GetPositionRootSpace(Bones[0])).sqrMagnitude >= CompleteLength * CompleteLength)
        {
            //Stretch
            var direction = (targetPosition - Positions[0]).normalized;

            //Setze alles nach Root
            for (int i = 1; i < Positions.Length; i++)
                Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
        }
        else
        {
            for (int i = 0; i < Positions.Length - 1; i++)
                Positions[i + 1] = Vector3.Lerp(Positions[i + 1], Positions[i] + StartDirectionSucc[i], SnapBackStrength);

            for (int iteration = 0; iteration < Iterations; iteration++)
            {
                //https://www.youtube.com/watch?v=UNoX65PRehA

                // Zur¸ck
                for (int i = Positions.Length - 1; i > 0; i--)
                {
                    if (i == Positions.Length - 1)
                        Positions[i] = targetPosition;
                    else
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i]; //set in line on distance
                }

                // Nach vorn
                for (int i = 1; i < Positions.Length; i++)
                    Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i - 1];

                // Nah genug:
                if ((Positions[Positions.Length - 1] - targetPosition).sqrMagnitude < Delta * Delta)
                    break;
            }
        }

        // Bewege zum Pole
        if (Pole != null)
        {
            var polePosition = GetPositionRootSpace(Pole);
            for (int i = 1; i < Positions.Length - 1; i++)
            {
                var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(polePosition);
                var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1], plane.normal);
                Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];
            }
        }

        // Setze Position & Rotation
        for (int i = 0; i < Positions.Length; i++)
        {
            if (i == Positions.Length - 1)
                SetRotationRootSpace(Bones[i], Quaternion.Inverse(targetRotation) * StartRotationTarget * Quaternion.Inverse(StartRotationBone[i]));
            else
                SetRotationRootSpace(Bones[i], Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) * Quaternion.Inverse(StartRotationBone[i]));
            SetPositionRootSpace(Bones[i], Positions[i]);
        }
    }

    private Vector3 GetPositionRootSpace(Transform current)
    {
        if (Root == null)
            return current.position;
        else
            return Quaternion.Inverse(Root.rotation) * (current.position - Root.position);
    }

    private void SetPositionRootSpace(Transform current, Vector3 position)
    {
        if (Root == null)
            current.position = position;
        else
            current.position = Root.rotation * position + Root.position;
    }

    private Quaternion GetRotationRootSpace(Transform current)
    {
        if (Root == null)
            return current.rotation;
        else
            return Quaternion.Inverse(current.rotation) * Root.rotation;
    }

    private void SetRotationRootSpace(Transform current, Quaternion rotation)
    {
        if (Root == null)
            current.rotation = rotation;
        else
            current.rotation = Root.rotation * rotation;
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var current = this.transform;
        for (int i = 0; i < ChainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.white;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }

        //Pole
        if(Pole != null)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(Pole.position, Vector3.up, 0.03f);
            Handles.DrawWireDisc(Pole.position, Vector3.forward, 0.03f);
            Handles.DrawWireDisc(Pole.position, Vector3.right, 0.03f);
        }
#endif
    }

}