using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameView : MonoBehaviour {

    public InGameController igCtrl;
    public Text console;
	
	// Update is called once per frame
	void Update () {
	    if(igCtrl.IgModel.IsDirty)
        {
            if(!string.IsNullOrEmpty(igCtrl.IgModel.ErrorMessage))
            {
                //rise popup error (must be added on the scene)
                return;
            }

            if(igCtrl.IgModel.Winner != 0)
            {
                //we've got a winner => show result (must be added to the scene)
                return;
            }

            //sinon c'est qu'on a un retour de command 
            //donc on met la console a jour

            igCtrl.IgModel.IsDirty = false;
        }
	}

    public void SubmitCommand()
    {
        igCtrl.SendCommand(console.text);
    }
}
