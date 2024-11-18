using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuintusSelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField] public Material QuintusMaterial;
    [SerializeField] public Material defaultMaterial;
    [SerializeField] public Material reflectiveBlackMaterial;
    [SerializeField] public Material redSkybox;
    [SerializeField] public Color QuintusLightColor = Color.red;

    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private Material originalSkybox;
    private Dictionary<Light, Color> originalLightColors = new Dictionary<Light, Color>();


    public void OnSelect(Transform selection)
    {
        var selectionRenderer = selection.GetComponent<Renderer>();
        if (selectionRenderer != null)
        {
            selectionRenderer.material = this.QuintusMaterial;

            originalSkybox = RenderSettings.skybox;

            RenderSettings.skybox = redSkybox;

            var lights = FindObjectsOfType<Light>();
            foreach (var light in lights)
            {
                if (!originalLightColors.ContainsKey(light))
                {
                    originalLightColors[light] = light.color;
                }
                light.color = QuintusLightColor;
            }


            var allRenderers = FindObjectsOfType<Renderer>();//Get every object to turn black
            foreach (var renderer in allRenderers)
            {
                if (renderer.transform != selection)
                {
                    //save original materials
                    if (!originalMaterials.ContainsKey(renderer))
                    {
                        originalMaterials[renderer] = renderer.sharedMaterials;
                    }
                    
                    // use black material
                    var blackMaterials = new Material[renderer.sharedMaterials.Length];
                    for (int i = 0; i < blackMaterials.Length; i++)
                    {
                        blackMaterials[i] = reflectiveBlackMaterial;
                    }
                    renderer.materials = blackMaterials;
                }
            }
        }
    }

    public void OnDeselect(Transform selection)
    {
        var selectionRenderer = selection.GetComponent<Renderer>();
        if (selectionRenderer != null)
        {
            //Unapply black
            selectionRenderer.material = this.defaultMaterial;
            foreach (var kvp in originalMaterials)
            {
                if (kvp.Key != null) // Ensure the renderer still exists
                {
                    kvp.Key.materials = kvp.Value;
                }
            }

            originalMaterials.Clear();

            RenderSettings.skybox = originalSkybox;

                        // Revert light colors
            foreach (var kvp in originalLightColors)
            {
                if (kvp.Key != null) // Ensure the light still exists
                {
                    kvp.Key.color = kvp.Value;
                }
            }

            originalLightColors.Clear();

        }
    }
}