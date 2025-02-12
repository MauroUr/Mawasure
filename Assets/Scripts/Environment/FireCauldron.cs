using UnityEngine;

public class FireCauldron : MonoBehaviour
{
    [SerializeField] private GameObject thisFire;
    [SerializeField] private GameObject otherFire;
    [SerializeField] private GameObject fence;
    [SerializeField] private Outline thisOutliner;
    [SerializeField] private Spawner spawner;
    private void OnTriggerEnter(Collider other)
    {
        if (other is CapsuleCollider)
            AdvancePuzzle(true);
    }

    public void AdvancePuzzle(bool shouldSpawn)
    {
        thisFire.SetActive(true);
        this.thisOutliner.enabled = false;

        if (thisFire.activeSelf && otherFire.activeSelf)
        {
            fence.SetActive(false);
            if(shouldSpawn)
                spawner.Spawn();
        }
    }

    public void RestartPuzzle()
    {
        thisFire.SetActive(false);
        this.thisOutliner.enabled = true;
        fence.SetActive(true);
    }

    public bool IsPuzzleOn()
    {
        return !(thisFire.activeSelf && otherFire.activeSelf);
    }
}
