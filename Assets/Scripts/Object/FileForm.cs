using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

public class FileForm : MonoBehaviour {
    [SerializeField] private Text selectedFile;
    [SerializeField] private String defFilter = ".*";
    [SerializeField] private List<String> filters;
    [SerializeField] private String title;
    
    public delegate void Selected(string title,String url);

    public delegate void Delete(String title);

    public event Selected OnSelected;
    public event Delete OnDeleted;

    private String fileName = "NONE";

    public string FileName {
        get { return fileName; }
        set {
            fileName = value;
            FileNames.setName(title,FileNames.urlToFileName(fileName));
        }
    }

    public string Title {
        get{
            return title;
        }
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

    public void onDelete() {
        FileNames.deleteName(title);
        selectedFile.text = FileNames.getName(title);
        if (OnDeleted != null)
            OnDeleted(title);
    }

    public void onSoundChoosed(string url) {
        var name = FileNames.urlToFileName(url);
        FileNames.setName(title,name);
        selectedFile.text = name;
        if (OnSelected != null)
            OnSelected(title,url);
    }
}
