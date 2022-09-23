using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image[] EnergyPoint;
    public float energy, maxEnergy,time,currentEnergy,H,S,V,maxTime;
    float lerpSpeed;
    // Start is called before the first frame update
    void Start()
    {
        energy=0;
        time=0;
        maxEnergy=6f;
        maxTime=12f;
    }

    // Update is called once per frame
    void Update()
    {
        lerpSpeed = 10f*Time.deltaTime;
        if(time<maxTime)
        {
            energy=Mathf.Lerp(0,maxEnergy,time/maxTime);
            time+=Time.deltaTime;
        }

        EnergyBarFiller();
        
    }
    public void EnergyBarFiller()
    {
        for(int i= 0;i<EnergyPoint.Length;i++)
        {
            //EnergyPoint[i].enabled= !DisplayEnergyPoint(energy,i);
            if(EnergyPoint[i].enabled==true)
            {
                currentEnergy= energy-i;
                EnergyPoint[i].fillAmount= Mathf.Lerp(EnergyPoint[i].fillAmount,currentEnergy,lerpSpeed);
                Color.RGBToHSV(EnergyPoint[i].color,out H,out S,out V);
                EnergyPoint[i].color= Color.Lerp(Color.HSVToRGB(H,S,0.69f),Color.HSVToRGB(H,S,0.9f),currentEnergy);
            }
        }
    }
    bool DisplayEnergyPoint(float _energy, int pointnumber)
    {
        return((pointnumber*1>=_energy));
    }
    
}
