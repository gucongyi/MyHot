using System;
using System.Reflection;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class UIFactory
    {
        public async Task<UI> CreateAsync(string type)
        {
	        try
	        {

                //MethodInfo mi = ui.GetType().GetMethod("AddUiComponentAmbiguousMatch").MakeGenericMethod(typeComponent);
                //mi.Invoke(ui, null);
                return await LoadBundleAsync(type);
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}

        public async Task<UI> LoadBundleAsync(string type)
        {
            Type typeComponent = UIType.GetComponentTypeByString(type);
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            await resourcesComponent.LoadBundleAsync($"ui/{type}.unity3d");
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"ui/{type}.unity3d", $"{type}");
            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
            go.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = ComponentFactory.Create<UI, GameObject>(go);
            ui.AddUiComponent(typeComponent);
            return ui;
        }
        
        public void Remove(string type)
	    {
			ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"ui/{type}.unity3d");
	    }
    }
}