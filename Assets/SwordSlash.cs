using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public Transform sword;
    private bool isSlashing;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSlashing)
        {
            StartCoroutine(Slash());
        }
    }

    System.Collections.IEnumerator Slash()
    {
        isSlashing = true;

        Quaternion startRot = sword.localRotation;
        Quaternion endRot = Quaternion.Euler(0, 0, -120);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 10;
            sword.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 10;
            sword.localRotation = Quaternion.Slerp(endRot, startRot, t);
            yield return null;
        }

        isSlashing = false;
    }
}