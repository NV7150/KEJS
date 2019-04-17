using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

public class FileForm : MonoBehaviour {
    [SerializeField] private Text selectedFile;
    [SerializeField] private String title;
    [SerializeField] private String defFilter = ".*";
    [SerializeField] private List<String> filters;
    
    public delegate void Selected(String url);

    public event Selected OnSelected;

    private String fileName = "NONE";

    public string FileName {
        get { return fileName; }
    }

    private void Start() {
        selectedFile.text = FileNames.getName(title);
    }

    public void onPushed() {
        if (!FileBrowser.IsOpen) {
            FileBrowser.SetFilters(true, new FileBrowser.Filter(title, filters.ToArray()));
            FileBrowser.SetDefaultFilter(defFilter);
            FileBrowser.ShowLoadDialog(onSoundChoosed, () => { }, title: "Choose Sound");
        }
    }

    private void onSoundChoosed(String url) {
        var name = FileNames.urlToFileName(url);
        FileNames.setName(title,name);
        selectedFile.text = name;
        if (OnSelected != null)
            OnSelected(url);
    }
}
