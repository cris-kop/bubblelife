using UnityEngine;

public class WorldTransitionEffect : MonoBehaviour
{
    [SerializeField] private Camera _lightWorldCam;
    [SerializeField] private Camera _darkWorldCam;
    [SerializeField] private GameObject _player2;

    [SerializeField] private Material _transitionMat;
    [SerializeField] private float _transitionDuration = 0.8f;

    private GameController.worldMode _currentMode;
    private float _switchTimeStamp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var controller = FindFirstObjectByType<GameController>();
        if (controller != null)
        {
            controller.OnWorldModeChanged += transitionToWorld;
        }
        else
        {
            transitionToWorld(GameController.worldMode.light);
        }

        _lightWorldCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 16);
        _darkWorldCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 16);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            transitionToWorld(GameController.worldMode.light);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            transitionToWorld(GameController.worldMode.dark);
        }
    }

    private void transitionToWorld(GameController.worldMode currentMode)
    {
        _currentMode = currentMode;
        _switchTimeStamp = Time.time;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {


        // _lightWorldCam.targetTexture

        // To overwrite the entire screen

        var timePassed = Time.time - _switchTimeStamp;
        var progress = Mathf.Clamp01(timePassed / _transitionDuration);
        _transitionMat.SetFloat("_Progress", progress);
        _transitionMat.SetFloat("_AspectRatio", (float)Screen.width / Screen.height);


        var center = _lightWorldCam.WorldToViewportPoint(_player2.transform.position);

        _transitionMat.SetFloat("_Light", _currentMode == GameController.worldMode.light ? 1 : 0);

        var from = _currentMode == GameController.worldMode.light ? _darkWorldCam.targetTexture : _lightWorldCam.targetTexture;
        _transitionMat.SetTexture("_FromTex", from);
        var to = _currentMode == GameController.worldMode.light ? _lightWorldCam.targetTexture : _darkWorldCam.targetTexture;
        _transitionMat.SetTexture("_ToTex", to);
        Debug.Log(center);
        _transitionMat.SetVector("_Center", new Vector4(center.x, center.y, 0, 0));

        Graphics.Blit(src, dest, _transitionMat);

        // Or to overwrite only what this specific Camera renders
        //Graphics.Blit(replacement, dest);
    }
}
