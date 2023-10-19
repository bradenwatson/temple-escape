using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;

public class PillarDescend : MonoBehaviour
{
    public AnimationCurve descendCurve;
    public float descentDuration = 5.0f;
    float elapsedTime = 0.0f;
    Vector3 initialPos;
    public bool enableDescend;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        descendCurve = new AnimationCurve();

        descendCurve.AddKey(new Keyframe(0, 0));
        
        descendCurve.AddKey(new Keyframe(5, 5));

        Keyframe keyframe1 = descendCurve.keys[0];
        Keyframe keyframe2 = descendCurve.keys[1];
        keyframe1.outTangent = 0;
        keyframe2.inTangent = 2;
        descendCurve.MoveKey(0, keyframe1);
        descendCurve.MoveKey(1, keyframe2);



    }

    public void DescendPillar()
    {
        enableDescend = true;
        if (elapsedTime < descentDuration)
        {
            float t = elapsedTime / descentDuration;
            float yPos = initialPos.y = descendCurve.Evaluate(t);
            transform.position = new Vector3(initialPos.x, yPos, initialPos.z);
        }
    }

    void Update()
    {
        if (enableDescend)
        {
            elapsedTime += Time.deltaTime;
        }
    }


}
