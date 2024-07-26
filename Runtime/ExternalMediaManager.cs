using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SimpleFileBrowser;
using UnityEngine.Networking;

namespace UnityExternalMediaManager
{
    public class ExternalMediaManager
    {
        protected List<string> tmpCopies;

        public ExternalMediaManager()
        {
            tmpCopies = new List<string>();
        }

        protected string GetTmpCopy(string sourcePath)
        {
            if (0 != sourcePath.IndexOf("content://com.android."))
            {
                return sourcePath;
            }

            var tmpDir = Application.persistentDataPath;
            string fileName = FileBrowserHelpers.GetFilename(sourcePath);
            string tmpPath = tmpDir + "/" + fileName;

            //todo: try-catch?
            FileBrowserHelpers.CopyFile(sourcePath, tmpPath);

            tmpCopies.Add(tmpPath);

            return tmpPath;
        }

        protected void DeleteTmpCopy(string tmpPath)
        {
            FileBrowserHelpers.DeleteFile(tmpPath);
        }

        public void Cleanup()
        {
            for (int i = 0; i < tmpCopies.Count; i++)
            {
                DeleteTmpCopy(tmpCopies[i]);
            }

            tmpCopies.Clear();
        }

        protected void DisplayError(string error, Text debugUI = null)
        {
            Debug.LogError("ExternalMediaManager error: " + error);

            if (null != debugUI)
            {
                debugUI.text += "\nExternalMediaManager error: " + error;
            }
        }

        #region SetText
        public void SetText(System.Action<string> action = null, Text textUI = null, Text debugUI = null)
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Texts (TXT, MD, JSON)", ".txt", ".md", ".json"));
            FileBrowser.SetDefaultFilter(".txt");

            var go = ExternalMediaManagerCorutines.GO;
            if (null == go)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene", debugUI);
                return;
            }
            var co = go.GetComponent<ExternalMediaManagerCorutines>();
            if (null == co)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene as component", debugUI);
                return;
            }
            co.StartCoroutine(ShowLoadTextDialogCoroutine(action, textUI, debugUI));
        }

        protected IEnumerator ShowLoadTextDialogCoroutine(System.Action<string> action = null, Text textUI = null, Text debugUI = null)
        {
            yield return FileBrowser.WaitForLoadDialog(
                FileBrowser.PickMode.Files,
                false,
                null,
                null,
                "Файл с текстом",
                "Выбрать"
            );

            if (FileBrowser.Success)
            {
                if (0 < FileBrowser.Result.Length)
                {
                    string textPath = FileBrowser.Result[0];

                    Debug.Log("Text path selected " + textPath);

                    if (null != action)
                    {
                        action(textPath);
                    }

                    if (null != textUI)
                    {
                        LoadText(textPath, textUI, debugUI);
                    }
                }
            }
            else
            {
                Debug.Log("Text path selecting error");
            }
        }
        #endregion

        #region LoadText
        public void LoadText(string textPath, Text textUI, Text debugUI = null)
        {
            if (null != debugUI)
            {
                debugUI.text += "\nExternalMediaManager::LoadText path: " + textPath;
            }

            textUI.text = FileBrowserHelpers.ReadTextFromFile(textPath);
        }

        public string GetText(string textPath)
        {
            return FileBrowserHelpers.ReadTextFromFile(textPath);
        }
        #endregion

        #region SetVideo
        public void SetVideo(System.Action<string> action = null, VideoPlayer videoUI = null, Text debugUI = null)
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Videos (MP4, MPEG)", ".mp4", ".mpg", ".mpeg"));
            FileBrowser.SetDefaultFilter(".mp4");

            var go = ExternalMediaManagerCorutines.GO;
            if (null == go)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene", debugUI);
                return;
            }
            var co = go.GetComponent<ExternalMediaManagerCorutines>();
            if (null == co)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene as component", debugUI);
                return;
            }
            co.StartCoroutine(ShowLoadVideoDialogCoroutine(action, videoUI, debugUI));
        }

        protected IEnumerator ShowLoadVideoDialogCoroutine(System.Action<string> action = null, VideoPlayer videoUI = null, Text debugUI = null)
        {
            yield return FileBrowser.WaitForLoadDialog(
                FileBrowser.PickMode.Files,
                false,
                null,
                null,
                "Файл видео",
                "Выбрать"
            );

            if (FileBrowser.Success)
            {
                if (0 < FileBrowser.Result.Length)
                {
                    string videoPath = FileBrowser.Result[0];

                    Debug.Log("Video path selected " + videoPath);

                    if (null != action)
                    {
                        action(videoPath);
                    }

                    if (null != videoUI)
                    {
                        LoadVideo(videoPath, videoUI, debugUI);
                    }
                }
            }
            else
            {
                Debug.Log("Video path selecting error");
            }
        }
        #endregion

        #region LoadVideo
        public void LoadVideo(string videoPath, VideoPlayer videoUI, Text debugUI = null)
        {
            if (null != debugUI)
            {
                debugUI.text += "\nExternalMediaManager::LoadVideo URL: " + videoPath;
            }

            videoUI.source = VideoSource.Url;
            videoUI.url = GetTmpCopy(videoPath);
        }
        #endregion

        #region SetAudio
        public void SetAudio(System.Action<string> action = null, AudioSource audioUI = null, Text debugUI = null)
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Audios (MP3, WAV)", ".mp3", ".wav"));
            FileBrowser.SetDefaultFilter(".mp3");

            var go = ExternalMediaManagerCorutines.GO;
            if (null == go)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene", debugUI);
                return;
            }
            var co = go.GetComponent<ExternalMediaManagerCorutines>();
            if (null == co)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene as component", debugUI);
                return;
            }
            co.StartCoroutine(ShowLoadAudioDialogCoroutine(action, audioUI, debugUI));
        }

        protected IEnumerator ShowLoadAudioDialogCoroutine(System.Action<string> action = null, AudioSource audioUI = null, Text debugUI = null)
        {
            yield return FileBrowser.WaitForLoadDialog(
                FileBrowser.PickMode.Files,
                false,
                null,
                null,
                "Файл аудио",
                "Выбрать"
            );

            if (FileBrowser.Success)
            {
                if (0 < FileBrowser.Result.Length)
                {
                    string audioPath = FileBrowser.Result[0];

                    Debug.Log("Audio path selected " + audioPath);

                    if (null != action)
                    {
                        action(audioPath);
                    }

                    if (null != audioUI)
                    {
                        LoadAudio(audioPath, audioUI, debugUI);
                    }
                }
            }
            else
            {
                Debug.Log("Audio path selecting error");
            }
        }
        #endregion

        #region LoadAudio
        public void LoadAudio(string audioPath, AudioSource audioUI, Text debugUI = null)
        {
            //load via bytes
            //audioUI.clip = LoadAudio(audioPath);
            //return;

            //load via UnityWebRequest
            var go = ExternalMediaManagerCorutines.GO;
            if (null == go)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene", debugUI);
                return;
            }
            var co = go.GetComponent<ExternalMediaManagerCorutines>();
            if (null == co)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene as component", debugUI);
                return;
            }
            co.StartCoroutine(LoadAudioClip(GetTmpCopy(audioPath), audioUI, debugUI));
        }

        protected IEnumerator LoadAudioClip(string audioPath, AudioSource audioUI, Text debugUI = null)
        {
            if (0 != audioPath.IndexOf("content://com.android.") && 0 != audioPath.IndexOf("file://"))
            {
                audioPath = "file://" + audioPath;
            }

            //GetAudioClip not loads content://...
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                    if (null != debugUI)
                    {
                        debugUI.text += "\nExternalMediaManager::LoadAudioClip error: " + www.error
                                        + "\nURL: " + www.url;
                    }
                }
                else
                {
                    audioUI.clip = DownloadHandlerAudioClip.GetContent(www);

                    if (null != debugUI)
                    {
                        debugUI.text += "\nExternalMediaManager::LoadAudioClip URL: " + www.url;
                    }
                }
            }
        }

        protected AudioClip LoadAudio(string audioPath)
        {
            if (!FileBrowserHelpers.FileExists(audioPath))
            {
                return null;
            }

            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(audioPath);

            //read by bytes, not worked, only noize
            /*
             * see https://forum.unity.com/threads/byte-to-audioclip.911723/
             * 
             */
            int samplesLength = (0 == bytes.Length % 4) ? bytes.Length / 4 : bytes.Length / 4 + 1;

            float[] samples = new float[samplesLength]; //size of a float is 4 bytes

            System.Buffer.BlockCopy(bytes, 0, samples, 0, bytes.Length);

            int channels = 1;
            int sampleRate = 44100;

            AudioClip clip = AudioClip.Create("ClipName", samples.Length, channels, sampleRate, false);
            clip.SetData(samples, 0);

            return clip;
        }
        #endregion

        #region SetImage
        public void SetImage(System.Action<string> action = null, Image imageUI = null, Text debugUI = null)
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Images (PNG, JPEG)", ".png", ".jpg", ".jpeg"));
            FileBrowser.SetDefaultFilter(".png");

            var go = ExternalMediaManagerCorutines.GO;
            if (null == go)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene", debugUI);
                return;
            }
            var co = go.GetComponent<ExternalMediaManagerCorutines>();
            if (null == co)
            {
                DisplayError("Script ExternalMediaManagerCorutines not mounted to scene as component", debugUI);
                return;
            }
            co.StartCoroutine(ShowLoadImageDialogCoroutine(action, imageUI, debugUI));
        }

        protected IEnumerator ShowLoadImageDialogCoroutine(System.Action<string> action = null, Image imageUI = null, Text debugUI = null)
        {
            yield return FileBrowser.WaitForLoadDialog(
                FileBrowser.PickMode.Files,
                false,
                null,
                null,
                "Файл изображения",
                "Выбрать"
            );

            if (FileBrowser.Success)
            {
                if (0 < FileBrowser.Result.Length)
                {
                    string imagePath = FileBrowser.Result[0];

                    Debug.Log("Image path selected " + imagePath);

                    if (null != action)
                    {
                        action(imagePath);
                    }

                    if (null != imageUI)
                    {
                        LoadImage(imagePath, imageUI, debugUI);
                    }
                }
            }
            else
            {
                Debug.Log("Image path selecting error");
            }
        }
        #endregion

        #region LoadImage
        public void LoadImage(string imagePath, Image imageUI, Text debugUI = null)
        {
            Vector2 imageSize = imageUI.rectTransform.sizeDelta;
            imageUI.sprite = LoadImage(imagePath, (int)imageSize.x, (int)imageSize.y);
        }

        protected Sprite LoadImage(string imagePath, int width, int height)
        {
            if (!FileBrowserHelpers.FileExists(imagePath))
            {
                return null;
            }

            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(imagePath);

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);

            texture.LoadImage(bytes);

            Sprite sprite = Sprite.Create(ResizeTexture2D(texture, width, height), new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));

            return sprite;
        }

        protected Texture2D ResizeTexture2D(Texture2D texture, int width, int height)
        {
            RenderTexture rt = new RenderTexture(width, height, 24);
            RenderTexture.active = rt;

            Graphics.Blit(texture, rt);

            Texture2D result = new Texture2D(width, height);
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            result.Apply();

            return result;
        }
        #endregion
    }
}
