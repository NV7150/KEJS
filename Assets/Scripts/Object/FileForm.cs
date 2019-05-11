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
    [SerializeField] private SettingType type;
    [SerializeField] private SettingRoll roll;
    [SerializeField] private string title;
    
    public delegate void Selected(SettingType type,SettingRoll roll,String url);
    public delegate void Delete(SettingType type,SettingRoll roll);

    public event Selected OnSelected;
    public event Delete OnDeleted;

    private String fileName = "NONE";


    private void Start() {
        selectedFile.text = SettingPathBase.urlToFileName(SettingPathBase.getPath(type, roll));
    }

    public void onPushed() {
        if (!FileBrowser.IsOpen) {
            FileBrowser.SetFilters(true, new FileBrowser.Filter(title, filters.ToArray()));
            FileBrowser.SetDefaultFilter(defFilter);
            FileBrowser.ShowLoadDialog(onChoosed, () => { }, title: "Choose Sound");
        }
    }

    public void onDelete() {
        if (OnDeleted != null)
            OnDeleted(type,roll);
        selectedFile.text = SettingPathBase.urlToFileName(SettingPathBase.getPath(type, roll));
    }

    public void onChoosed(string url) {
        if (OnSelected != null)
            OnSelected(type,roll,url);
        selectedFile.text = SettingPathBase.urlToFileName(SettingPathBase.getPath(type, roll));
    }
}
