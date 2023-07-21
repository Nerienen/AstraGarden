using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionStateManager : MonoBehaviour
{
    private Liquid liquid;


    private Material myMaterial;

    public float fillAmount;

    public float amountThatFillsWithEveryBall = -0.1f;

    public float redIncrement = 0.25f;
    public float greenIncrement = 0.25f;
    public float blueIncrement = 0.25f;
    



    //bools for checking if its a potion of type x


    public bool isGluePotion = false;
    public bool isCleanserPotion = false;
    public bool isWaterPotion = false;

    public bool isManaPotion = false;
    public bool isFirePotion = false;

    public bool isMagicDetectionPotion = false;
    public bool isSpeedPotion = false;
    public bool isDarkVisionPotion = false;
    public bool isGoldStarPotion = false;
    public bool isGrowthPotion = false;
    public bool isPoisonPotion = false;

    public bool isPoisonResistancePotion = false;
    public bool isFireResistancePotion = false;

    




    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the Liquid script on this object
        liquid = GetComponent<Liquid>();
        if (liquid == null)
        {
            Debug.LogError("No Liquid component found on the same GameObject as PotionStateManager!");
        }


        fillAmount = liquid.fillAmount;

        
        
        Renderer renderer = GetComponent<Renderer>();
        myMaterial = renderer.material;
        Color color = myMaterial.GetColor("_Color");


        myMaterial = GetComponent<Renderer>().material;
        Color tint = myMaterial.GetColor("_Tint");
        Color topColor = myMaterial.GetColor("_TopColor");
        Color foamColor = myMaterial.GetColor("_FoamColor");
        Color rimColor = myMaterial.GetColor("_RimColor");

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        //FIRE

        if (other.CompareTag("FireLiquid"))
        {
            AddHalfFillAmount();
            
            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 1f, redIncrement),
                Mathf.MoveTowards(tint.g, 0f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0f, blueIncrement),
                tint.a
            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 0.76f && newColor.g < 0.3f && newColor.b < 0.3f)
            {
                isFirePotion = true;
            }
            else
            {
                isFirePotion = false;
            }

            // Disable the FireLiquid object
            other.gameObject.SetActive(false);
        }

        //GLUE

        if (other.CompareTag("GlueLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0.89f, redIncrement),
                Mathf.MoveTowards(tint.g, 0.82f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0.82f, blueIncrement),
                tint.a
            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 0.85f && newColor.g > 0.8f && newColor.b > 0.8f)
            {
                isGluePotion = true;
            }
            else
            {
                isGluePotion = false;
            }

            other.gameObject.SetActive(false);
        }

        //WATER

        if (other.CompareTag("WaterLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0.59f, redIncrement),
                Mathf.MoveTowards(tint.g, 0.82f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0.87f, blueIncrement),
                tint.a
            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 0.55f && newColor.g > 0.8f && newColor.b > 0.85f && newColor.r < 0.7f)
            {
                isWaterPotion = true;
            }
            else
            {
                isWaterPotion = false;
            }
            other.gameObject.SetActive(false);
        }


        //CLEANSER

        if (other.CompareTag("CleanserLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0.7f, redIncrement),
                Mathf.MoveTowards(tint.g, 0.8f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0.85f, blueIncrement)
                
                
            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 0.65f && newColor.g > 0.75f && newColor.b > 0.8f)
            {
                isCleanserPotion = true;
            }
            else
            {
                isCleanserPotion = false;
            }
            other.gameObject.SetActive(false);
        }



        //MANA

        if (other.CompareTag("ManaLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0f, redIncrement),
                Mathf.MoveTowards(tint.g, 0.3f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0.83f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r < 0.1f && newColor.g < 0.35f && newColor.b > 0.8f)
            {
                isManaPotion = true;
            }
            else
            {
                isManaPotion = false;
            }
            other.gameObject.SetActive(false);
        }

        //MAGIC DETECTION

        if (other.CompareTag("MagicDetectionLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0.73f, redIncrement),
                Mathf.MoveTowards(tint.g, 0f, greenIncrement),
                Mathf.MoveTowards(tint.b, 3.57f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 0.7f && newColor.g < 0.1f && newColor.b > 3.3f)
            {
                isMagicDetectionPotion = true;
            }
            else
            {
                isMagicDetectionPotion = false;
            }
            other.gameObject.SetActive(false);
        }

        //SPEED 

        if (other.CompareTag("SpeedLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0.15f, redIncrement),
                Mathf.MoveTowards(tint.g, 0.9f, greenIncrement),
                Mathf.MoveTowards(tint.b, 1f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r < 0.19f && newColor.g > 0.8f && newColor.b > 0.9f)
            {
                isSpeedPotion = true;
            }
            else
            {
                isSpeedPotion = false;
            }
            other.gameObject.SetActive(false);
        }


        //DARK VISION 

        if (other.CompareTag("DarkVisionLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 2.4f, redIncrement),
                Mathf.MoveTowards(tint.g, 0.2f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 2.2f && newColor.g < 0.3f && newColor.b < 0.1f)
            {
                isDarkVisionPotion = true;
            }
            else
            {
                isDarkVisionPotion = false;
            }
            other.gameObject.SetActive(false);
        }

        //GOLD STAR 

        if (other.CompareTag("GoldStarLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 3.6f, redIncrement),
                Mathf.MoveTowards(tint.g, 2.5f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 3.5f && newColor.g > 2.3f && newColor.b < 0.1f)
            {
                isGoldStarPotion = true;
            }
            else
            {
                isGoldStarPotion = false;
            }
            other.gameObject.SetActive(false);
        }

        //GROWTH 

        if (other.CompareTag("GrowthLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0.4f, redIncrement),
                Mathf.MoveTowards(tint.g, 0.5f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 0.3f && newColor.r < 0.5f && newColor.g > 0.4f && newColor.g < 0.6 && newColor.b < 0.1f)
            {
                isGrowthPotion = true;
            }
            else
            {
                isGrowthPotion = false;
            }
            other.gameObject.SetActive(false);
        }

        //POISON 

        if (other.CompareTag("PoisonLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0f, redIncrement),
                Mathf.MoveTowards(tint.g, 1.9f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r < 0.1f && newColor.g > 1.6f && newColor.b < 0.1f)
            {
                isPoisonPotion = true;
            }
            else
            {
                isPoisonPotion = false;
            }
            other.gameObject.SetActive(false);
        }

        //POISON RESISTANCE 

        if (other.CompareTag("PoisonResistanceLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 0.75f, redIncrement),
                Mathf.MoveTowards(tint.g, 1f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0.55f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 0.7f && newColor.r < 0.8f && newColor.g > 0.9f && newColor.g < 1.1f && newColor.b > 0.5f && newColor.b < 0.6f)
            {
                isPoisonResistancePotion = true;
            }
            else
            {
                isPoisonResistancePotion = false;
            }
            other.gameObject.SetActive(false);
        } 

        //FIRE RESISTANCE

        if (other.CompareTag("FireResistanceLiquid"))
        {
            AddHalfFillAmount();

            Color tint = myMaterial.GetColor("_Tint");
            Color newColor = new Color(
                Mathf.MoveTowards(tint.r, 1f, redIncrement),
                Mathf.MoveTowards(tint.g, 0.5f, greenIncrement),
                Mathf.MoveTowards(tint.b, 0.5f, blueIncrement)


            );
            myMaterial.SetColor("_Tint", newColor);

            if (newColor.r > 0.9f && newColor.g > 0.4f && newColor.g < 0.6f && newColor.b > 0.45f && newColor.b < 0.6f )
            {
                isFireResistancePotion = true;
            }
            else
            {
                isFireResistancePotion = false;
            }
            other.gameObject.SetActive(false);
        }


    }

    public void AddHalfFillAmount()
    {
        liquid.IncreaseFillAmount(amountThatFillsWithEveryBall);
        
    }
}
