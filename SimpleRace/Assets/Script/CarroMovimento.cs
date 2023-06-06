using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CarroMovimento : MonoBehaviourPunCallbacks
{

    public WheelCollider[] wCollider;
    public Transform[] rodas;
    private Vector3 pos;
    private Quaternion rot;
    public Camera camErrada;

    public float torque, friccao, freio, ang;

    void Start()
    {  
        // Verifica e desabilita todas as cameras exceto a camera do player.
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            camErrada.enabled = false;
        }
        else
        {
            // Habilita a função ConfigureVehicleSubsteps em todos wellcoliders para aumentar a estabilade.
            for (int x = 0; x < wCollider.Length; x++)
            {
                wCollider[x].ConfigureVehicleSubsteps(1, 12, 15);
            }
        }
    }

    void Update()
    {
        Rodas(); // Chama a função para atualizar a posição das rodas.
    }

    // Movimenta o carro usando as teclas de setas.
    public void FixedUpdate()
    {
        if (photonView.CreatorActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            wCollider[0].steerAngle = ang * Input.GetAxis("Horizontal");
            wCollider[1].steerAngle = ang * Input.GetAxis("Horizontal");

            for (int x = 0; x < wCollider.Length; x++)
            {
                wCollider[x].motorTorque = Input.GetAxis("Vertical") * torque;
                wCollider[x].brakeTorque = (Input.GetKey(KeyCode.Space)) ? freio : friccao - Mathf.Abs(Input.GetAxis("Vertical") * friccao);
            }
        }
    }

    // Atualiza a posição das rodas enquanto o carro anda.
    public void Rodas()
    {
        if (photonView.CreatorActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            for (int x = 0; x < wCollider.Length; x++)
            {
                wCollider[x].GetWorldPose(out pos, out rot);
                rodas[x].position = pos;
                rodas[x].rotation = rot;

            }
        }
    }
}
