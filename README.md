# Unity External Media Manager (v1.0.0)

Manager of external media files (texts, images, audio, video) for Unity 3D.

## Requirements

*   Unity 3D
*   [UnitySimpleFileBrowser](https://github.com/yasirkula/UnitySimpleFileBrowser)

## Usage

```c#
    ...

    public Text TextOutputUI;

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

    ...
```
