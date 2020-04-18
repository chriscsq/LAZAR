using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorInfo : MonoBehaviour
{
    public bool replacePreviousPower = true;
    public LaserPowers reflectionPower = LaserPowers.DEFAULT;

    private Transform mountainTransform;
    private Transform thisTransform;
    private Transform parentTransform;

    private Vector3 originalLocalPos;
    private Vector3 originalLocalRot;

    void Start() {
        mountainTransform = GameObject.FindGameObjectWithTag("Mountain").GetComponent<Transform>();

        thisTransform = GetComponent<Transform>();
        parentTransform = thisTransform.parent;

        originalLocalPos = thisTransform.localPosition;
        originalLocalRot = thisTransform.localEulerAngles;

    }

    void LateUpdate() {
        // .forward for MountainTransform because of Blender's coordinate system stuff.
        float diffAngle = Vector3.SignedAngle(parentTransform.up, mountainTransform.forward, -parentTransform.forward);
        if (Mathf.Abs(diffAngle) < 10.0f && Mathf.Abs(diffAngle) > 0.1f) {
            //thisTransform.localEulerAngles = originalLocalRot - diffAngle * (new Vector3(diffAngle, 0, 0));
            thisTransform.rotation = Quaternion.FromToRotation(thisTransform.up, mountainTransform.forward) * thisTransform.rotation;
        }
        else {
            thisTransform.localEulerAngles = originalLocalRot;
        }

        float diff = parentTransform.position.y - mountainTransform.position.y;
        if (Mathf.Abs(diff) < 0.1f) {
            thisTransform.localPosition = originalLocalPos - (new Vector3(0, diff, 0));
        }
        else {
            thisTransform.localPosition = originalLocalPos;
        }
    }
}
