using UnityEngine;

public class Lock : MonoBehaviour
{

   

   
  

    private void InteractObject_InteractSuccessAction(object sender, System.EventArgs e)
    {
        Debug.Log("Unlock");
        Destroy(gameObject);
    }

  
}
