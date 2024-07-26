# Unity External Media Manager (v1.0.0)

Manager of external media files (texts, images, audio, video) for Unity 3D.
Allows to pick the media file (via UnitySimpleFileBrowser)
and make some action with media file path, media data
or loads media into UI element.

## Requirements

*   Unity 3D
*   [UnitySimpleFileBrowser](https://github.com/yasirkula/UnitySimpleFileBrowser)

## Usage

```c#
    ...

    public Text TextOutputUI;
    public Image ImageOutputUI;
    public AudioSource AudioSourceUI;
    public VideoPlayer VideoPlayerUI;

    protected ExternalMediaManager emm;

    // Start is called before the first frame update
    void Start()
    {
        emm = new ExternalMediaManager();
    }

    public void ButtonPickTextClick()
    {
        emm.SetText(
            (string path) =>
            {
                string text = emm.GetText(path);
                Debug.Log("Text loaded: " + text);

                //do something with loaded text or its file path
            },
            TextOutputUI
        );
    }

    public void ButtonPickImageClick()
    {
        emm.SetImage(
            (string path) =>
            {
                //do something with picked image file path
            },
            ImageOutputUI
        );
    }

    public void ButtonPickAudioClick()
    {
        emm.SetAudio(
            (string path) =>
            {
                //do something with picked audio file path
            },
            AudioSourceUI
        );
    }

    public void ButtonPickVideoClick()
    {
        emm.SetVideo(
            (string path) =>
            {
                //do something with picked video file path
            },
            VideoPlayerUI
        );
    }

    ...
```
