using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadScreenManager : MonoBehaviour
{
    //events
    readonly UnityEvent OnShow = new UnityEvent();
    readonly UnityEvent<float> OnProgression = new UnityEvent<float>();
    readonly UnityEvent OnHide = new UnityEvent();

    public bool Active { get; private set; }
    protected Camera CameraObject { get; private set; }
    // Start is called before the first frame update
    private void Awake()
    {
        //Object should be persistant
        DontDestroyOnLoad(this.gameObject);

        CameraObject = GetComponentInChildren<Camera>();
        if(!CameraObject)
        {
            this.CameraObject = this.gameObject.AddComponent<Camera>(); //just do it for them so we dont get weird states.
        }
        CameraObject.enabled = true; //assume cuz of persistance that this needs to be enabled
    }

    public void Show()
    {
        if(this.Active)
        {
            return; // dont start again if already up.
        }
        CameraObject.enabled = true;
        this.Active = true;
        // Alert every on we are updated.
        OnShow.Invoke();
    }

    public void Progress(float progress)
    {
        if(!this.Active)
        {
            return; //dont update progress if its not active.
        }

        // Alert every on we are updated.
        OnProgression.Invoke(progress);
    }

    public void Hide()
    {
        if(!this.Active)
        {
            return; // Dont hide if not started.
        }
        CameraObject.enabled = false;
        this.Active = false;

        // Alert every on we are updated.
        OnHide.Invoke();
    }

    protected void OnDestroy()
    {
        //remove all listeners
        this.OnShow.RemoveAllListeners();
        this.OnProgression.RemoveAllListeners();
        this.OnHide.RemoveAllListeners();
    }

}
