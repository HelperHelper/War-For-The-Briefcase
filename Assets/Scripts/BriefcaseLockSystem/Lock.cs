using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Lock : GameTrigger
{
    public string briefcaseType;
    public TMP_Text briefcaseNameText;


    Canvas canvas;

    void Start()
    {
        briefcaseNameText.text = briefcaseType;

        canvas = briefcaseNameText.GetComponentInParent<Canvas>();
        canvas.gameObject.SetActive(false);
    }


    public virtual void Opened()
    {
        Trigger();
    }

    void OnTriggerEnter(Collider other)
    {
        canvas.gameObject.SetActive(true);

        var briefcasechain = other.GetComponent<BriefcaseChain>();

        var timer = GameSystem.Instance.Minute;

      //Debug.Log("El tiempo que ha pasado es igual a minutos y segundos: " + timer + GameSystem.Instance.Second);
      

        if (briefcasechain != null && briefcasechain.HaveBriefcase(briefcaseType) && timer == 5)
        {
            briefcasechain.UseKey(briefcaseType);
            Opened();       
            //Solo destruye el script, si esta en la puerta no queremos destruir la puerta.
            Destroy(this);
            Destroy(canvas.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        canvas.gameObject.SetActive(false);
    }

}

//#if UNITY_EDITOR
//[CustomEditor(typeof(Lock))]
//public class LockEditor : Editor
//{
//    SerializedProperty m_ActionListProperty;
//    SerializedProperty m_BriefcaseNameTextProperty;
//    Lock m_Lock;

//    int m_BriefcaseTypeIndex = -1;
//    string[] m_AllBriefcaseType = new string[0];

//    void OnEnable()
//    {
//        m_Lock = target as Lock;
//        m_ActionListProperty = serializedObject.FindProperty("actions");
//        m_BriefcaseNameTextProperty = serializedObject.FindProperty("BriefcaseNameText");

//        var allBriefcase = Resources.FindObjectsOfTypeAll<Briefcase>();
//        foreach (var briefcase in allBriefcase)
//        {
//            ArrayUtility.Add(ref m_AllBriefcaseType, briefcase.briefcaseType);

//            if (m_Lock.briefcaseType == briefcase.briefcaseType)
//            {
//                m_BriefcaseTypeIndex = m_AllBriefcaseType.Length - 1;
//            }
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        EditorGUILayout.PropertyField(m_BriefcaseNameTextProperty);
//        EditorGUILayout.PropertyField(m_ActionListProperty, true);

//        if (m_AllBriefcaseType.Length > 0)
//        {
//            int index = EditorGUILayout.Popup("Briefcase Type", m_BriefcaseTypeIndex, m_AllBriefcaseType);
//            if (index != m_BriefcaseTypeIndex)
//            {
//                Undo.RecordObject(m_Lock, "Changed Briefcase Type");

//                m_Lock.briefcaseType = m_AllBriefcaseType[index];
//                m_BriefcaseTypeIndex = index;
//            }
//        }
//        else
//        {
//            EditorGUILayout.HelpBox("Add at least a Briefcase in the scene to be able to select the type here", MessageType.Warning);
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}
//#endif
