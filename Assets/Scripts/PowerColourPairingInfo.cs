using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerColourPairingInfo : MonoBehaviour
{
    [System.Serializable]
    public struct PowerColourPairing {
        public LaserPowers laserPower;
        public Color laserColour;    
    }
    // Unity does not expose Dictionaries in the inspector.
    // Therefore, the following workaround is used: https://answers.unity.com/questions/642431/dictionary-in-inspector.html
    public PowerColourPairing[] powerColourPairingInfo;

    private Dictionary<LaserPowers, Color> powerColourPairingDict;

    // Start is called before the first frame update
    void Start()
    {
        powerColourPairingDict = new Dictionary<LaserPowers, Color>();
        foreach(PowerColourPairing p in powerColourPairingInfo) {
            powerColourPairingDict[p.laserPower] = p.laserColour;
        }
    }

    public Color GetAssociatedColor(LaserPowers power) {
        Color c = powerColourPairingDict[power];
        return new Color(c.r, c.g, c.b, c.a);
    }

}
