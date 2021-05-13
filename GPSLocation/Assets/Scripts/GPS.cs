using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    public Text GPSstatus;
    public Text latitudeValue;
    public Text longitudeValue;
    public Text altitudeValue;
    public Text horizontalAccuracyValue;
    public Text timeStampValue;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLocationService());
    }
    private IEnumerator StartLocationService(){
        //verificamos si el usuario tiene servicio de localizacion dispoonible
        if(!Input.location.isEnabledByUser){
            Debug.Log("Usuario no tiene activado el GPS");
            yield break;
        }
        //start service before querying location
        Input.location.Start();
        //wait until service initialize
        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0){
            yield return new WaitForSeconds(1);
            maxWait --; 
        }
        //service didn't init in 20 sec
        if(maxWait < 1 ){
            Debug.Log("Timed out");
            GPSstatus.text ="tiempo fuera";
            yield break;
        }
        // connection failed
        if(Input.location.status == LocationServiceStatus.Failed){
            Debug.Log("Unable to determin device location");
            GPSstatus.text = "No se puede determinar la ubicación del dispositivo";
            yield break;
        }else{
            // accsess granted
            GPSstatus.text = "Ejecutando";
            InvokeRepeating("UpdateGPSData", 0.5f,1f);
        }
    }//end of GPSLoc
    private void UpdateGPSData() {
        if(Input.location.status == LocationServiceStatus.Running){
            //access granted to gps value and it has been init
            GPSstatus.text = "Ejecutando";
            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            timeStampValue.text = Input.location.lastData.timestamp.ToString();
 
        }else{
// service is stoped
        GPSstatus.text = "Stop";
        }
    }

} //end of GPSLocation
