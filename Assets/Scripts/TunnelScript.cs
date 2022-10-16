using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class TunnelScript : MonoBehaviour
{
    [SerializeField]
    List<Transform> tunnelObj;

    public float tunnelRealSpeed = -1f;

    float tunnelSpeed;
    public float TunnelSpeed   
    {
        get { return tunnelSpeed; }   
        set
        {
            tunnelSpeed = value;
        }  
    }

    bool CoolDown;
    public static TunnelScript Instance;

    [SerializeField] Color[] colors;
    [SerializeField] Material TunnelMat;

    float hueValue;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        tunnelSpeed = tunnelRealSpeed;

        //hueValue = Random.Range(0, 1f);
        //foreach (Transform t in tunnelObj)
        //{
        //    ChangeColor(t.GetComponent<MeshRenderer>());
        //}
    }

    void ChangeColor(MeshRenderer MR)
    {
        MR.material.color = UnityEngine.Color.HSVToRGB(hueValue, 0.6f, 0.8f);
        hueValue += 0.08f;
        if (hueValue >= 1)
        {
            hueValue = 0;
        }
    }

    private void Start()
    {
        SetLvlSpeed();
    }

    public void SetLvlSpeed()
    {
        tunnelRealSpeed = LevelManager.Instance.LevelSettings[LevelManager.PlayerLvl()].speed;
        tunnelSpeed = tunnelRealSpeed;
    }
    void FixedUpdate()
    {
        if (PlayerController.Instance.StartGame)
        {
            transform.Translate(0, 0, TunnelSpeed * Time.deltaTime);

            if (tunnelObj[0].transform.position.z < -75f)
            {
                Transform t = tunnelObj[0];
                tunnelObj.Remove(t);

                t.transform.position = tunnelObj[tunnelObj.Count - 1].position + new Vector3(0, 0, 60);

                tunnelObj.Add(t);
                //ChangeColor(t.GetComponent<MeshRenderer>());
            }
        }
    }

    public void StopCooldown()
    {
        StopAllCoroutines();
        CoolDown = false;
    }

    public void StartCooldown()
    {
        if (!CoolDown)
        {
            StartCoroutine(SpeedCooldown());
        }
    }

    IEnumerator SpeedCooldown()
    {
        CoolDown = true;
        yield return new WaitForSeconds(7f);

        float d = tunnelSpeed;
        DOTween.To(() => tunnelSpeed, x => tunnelSpeed = x, tunnelRealSpeed, d).OnComplete(()=> { AudioManager.Instance.BG_MusicSpeed(false); });
    }

    public void ChangeTunnelMat()
    {
        SetLvlSpeed();

        int f = Random.Range(0, colors.Length);

        TunnelMat.color = new Color( colors[f].r, colors[f].g, colors[f].b,1);      
    }
}
