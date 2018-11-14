using System.Collections;

using System.Collections.Generic;

using UnityEngine;


public class Transparent : MonoBehaviour
{


    public GameObject Roof = null;


    void OnTriggerEnter(Collider collider)

    {

        if (IsCharacter(collider))

        {

            SetMaterialTransparent();

            iTween.FadeTo(Roof, 0.5f, 1);

        }

    }


    private bool IsCharacter(Collider collider)

    {
        
        PlayerController character = collider.gameObject.GetComponent<PlayerController>();
        return true;

    }


    private void SetMaterialTransparent()

    {

        foreach (Material m in Roof.GetComponent<Renderer>().materials)

        {

            m.SetFloat("_Mode", 1);

            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);

            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            m.SetInt("_ZWrite", 1);

            m.DisableKeyword("_ALPHATEST_ON");

            m.EnableKeyword("_ALPHABLEND_ON");

            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            m.renderQueue = 3000;

        }

    }


    private void SetMaterialOpaque()

    {

        foreach (Material m in Roof.GetComponent<Renderer>().materials)

        {

            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);

            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);

            m.SetInt("_ZWrite", 1);

            m.DisableKeyword("_ALPHATEST_ON");

            m.DisableKeyword("_ALPHABLEND_ON");

            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            m.renderQueue = -1;

        }

    }


    void OnTriggerExit(Collider collider)

    {

        if (IsCharacter(collider))

        {

            // Set material to opaque

            iTween.FadeTo(Roof, 1, 1);


            Invoke("SetMaterialOpaque", 1.0f);

        }

    }

}