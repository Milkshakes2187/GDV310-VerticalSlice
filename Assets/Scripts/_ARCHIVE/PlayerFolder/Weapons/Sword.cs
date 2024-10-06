//using Unity.VisualScripting;
//using UnityEngine;

//public class Sword : MonoBehaviour
//{

//    public string swordName;

//    public float fBaseDamage;

//    [SerializeField] GameObject effectSwordBossImpact;

//    BoxCollider hitBox;

//    Vector3 defaultHitboxSize;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    private void Start()
//    {
//        //assigning the hitbox
//        if(GetComponent<BoxCollider>()!= null)
//        {
//            hitBox = GetComponent<BoxCollider>();
//            defaultHitboxSize = hitBox.bounds.size;
//        }
//    }


    
//    private void OnTriggerEnter(Collider other)
//    {
//        if(other.name == "Player")
//        {
//            return;
//        }

//        var hitEnemy = other.gameObject.transform.root.GetComponent<PrototypeBoss>();
//        Debug.Log(other.name);
//        if(hitEnemy != null)
//        {
//            //DAMAGE THE ENEMY

//            hitEnemy.TakeDamage(fBaseDamage);

//            //SOUND
//            if (AudioLibrary.instance.audioSwordImpact.clip)
//            {
//                AudioLibrary.instance.audioSwordImpact.PlayOneShot(AudioLibrary.instance.audioSwordImpact.clip, 1);
//            }

//            //HIT EFFECT!
//            if (effectSwordBossImpact != null)
//            {
//                var effect = Instantiate(effectSwordBossImpact, other.ClosestPoint(hitBox.transform.position), transform.rotation);
//                Destroy(effect, 5);
//            }



//            //Lifesteal
//            if (PlayerCombat.instance.bLifestealActive)
//            {
//                Player.instance.TempHeal(Mathf.Round(PlayerCombat.instance.fLifestealPercent * fBaseDamage));

//                //SOUND
//                if (AudioLibrary.instance.audioSwordLifestealImpact.clip)
//                {
//                    AudioLibrary.instance.audioSwordLifestealImpact.PlayOneShot(AudioLibrary.instance.audioSwordLifestealImpact.clip, 1);
//                }
//            }

            

//            DisableHitbox();

//        }


//        var hitTest = other.gameObject.GetComponent<TestCube>();
//        if (hitTest != null)
//        {
//            //DAMAGE THE ENEMY
        
//            hitTest.TakeDamage(fBaseDamage);

//            DisableHitbox();
//            //destroy if dead? query if dead?

//        }
//    }

//    //turns on the sword hitbox
//    public void EnableHitbox()
//    {
//        hitBox.enabled = true;
//    }

//    //turns off the sword hitbox
//    public void DisableHitbox()
//    {
//        if(hitBox)
//        {
//            hitBox.enabled = false;
//            hitBox.transform.localScale = Vector3.one;
//        }
        
//    }


//    public void ScaleUpHitbox()
//    {
//        Vector3 hitboxSize = new( 1,  1.7f, 1 );
//        hitBox.transform.localScale = hitboxSize;
//    }
//}
