using UnityEngine;
using TMPro; // On utilise TextMeshPro, le standard pour le texte sur Unity
using System.Collections.Generic;

public class VRLogViewer : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI logTextDisplay;
    
    [Header("Settings")]
    public int maxLines = 15;

    private Queue<string> logQueue = new Queue<string>();

    void OnEnable()
    {
        // S'abonner aux événements de log de Unity
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Se désabonner pour éviter les fuites de mémoire
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Colorer le texte selon le type de log
        string color = "white";
        if (type == LogType.Error || type == LogType.Exception) 
            color = "red";
        else if (type == LogType.Warning) 
            color = "yellow";

        string formattedLog = $"<color={color}>{logString}</color>";
        logQueue.Enqueue(formattedLog);

        // Garder seulement les X dernières lignes pour ne pas surcharger l'écran
        if (logQueue.Count > maxLines)
        {
            logQueue.Dequeue();
        }

        // Mettre à jour l'affichage
        if (logTextDisplay != null)
        {
            logTextDisplay.text = string.Join("\n", logQueue.ToArray());
        }
    }
}