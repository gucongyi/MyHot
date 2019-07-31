using System;
using UnityEngine;
using UnityEditor;

namespace PSDUIImporter
{
    internal class InputFieldLayerImport : ILayerImport
    {
        private PSDImportCtrl pSDImportCtrl;

        public InputFieldLayerImport(PSDImportCtrl pSDImportCtrl)
        {
            this.pSDImportCtrl = pSDImportCtrl;
        }

        public void DrawLayer(Layer layer, GameObject parent)
        {
            UnityEngine.UI.InputField temp = AssetDatabase.LoadAssetAtPath(PSDImporterConst.ASSET_PATH_INPUTFIELD, typeof(UnityEngine.UI.InputField)) as UnityEngine.UI.InputField;
            UnityEngine.UI.InputField inputfield = GameObject.Instantiate(temp) as UnityEngine.UI.InputField;
            inputfield.transform.SetParent(parent.transform, false);//.parent = parent.transform;
            inputfield.name = layer.name;
            inputfield.placeholder?.gameObject.SetActive(false);
            //if (layer.image != null)
            if (layer.layers!=null)
            {
                for (int imageIndex = 0; imageIndex < layer.layers.Length; imageIndex++)
                {
                    PSImage image = layer.layers[imageIndex].image;
                    //PSImage image = layer.image;

                    if (image.imageType == ImageType.Label)
                    {
                        if (image.name.ToLower().Contains("text"))
                        {
                            UnityEngine.UI.Text text = (UnityEngine.UI.Text)inputfield.textComponent;//inputfield.transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
                            Color color;
                            if (UnityEngine.ColorUtility.TryParseHtmlString(("#" + image.arguments[0]), out color))
                            {
                                text.color = color;
                            }
                            int size;
                            float sizeFloat;
                            if(float.TryParse(image.arguments[2],out sizeFloat))
                            {
                                size = Mathf.RoundToInt(sizeFloat);
                                text.fontSize = size;
                            }
                            text.text = image.arguments[3];

                            //设置字体,注意unity中的字体名需要和导出的xml中的一致
                            string fontFolder;

                            if (image.arguments[1].ToLower().Contains("static"))
                            {
                                fontFolder = PSDImporterConst.FONT_STATIC_FOLDER;
                            }
                            else
                            {
                                fontFolder = fontFolder = PSDImporterConst.FONT_FOLDER;
                            }
                            string fontFullName = fontFolder + image.arguments[1] + PSDImporterConst.FONT_SUFIX;
                            Debug.Log("font name ; " + fontFullName);
                            var font = AssetDatabase.LoadAssetAtPath(fontFullName, typeof(Font)) as Font;
                            if (font == null)
                            {
                                Debug.LogWarning("Load font failed : " + fontFullName);
                            }
                            else
                            {
                                text.font = font;
                            }
                            //ps的size在unity里面太小，文本会显示不出来,暂时选择溢出
                            text.verticalOverflow = VerticalWrapMode.Overflow;
                            text.horizontalOverflow = HorizontalWrapMode.Overflow;
                            //设置对齐
                            if (image.arguments.Length >= 5)
                                text.alignment = ParseAlignmentPS2UGUI(image.arguments[4]);
                            else
                            {
                                text.alignment =  TextAnchor.MiddleLeft;
                            }
                            TextImport.SetOutline(image, text);
                        }
                        else if (image.name.ToLower().Contains("holder"))
                        {
                            /*
                            UnityEngine.UI.Text text = (UnityEngine.UI.Text)inputfield.placeholder;//.transform.Find("Placeholder").GetComponent<UnityEngine.UI.Text>();
                            Color color;
                            if (UnityEngine.ColorUtility.TryParseHtmlString(("#" + image.arguments[0]), out color))
                            {
                                text.color = color;
                            }

                            int size;
                            float sizeFloat;
                            if (float.TryParse(image.arguments[2], out sizeFloat))
                            {
                                size = Mathf.RoundToInt(sizeFloat);
                                text.fontSize = size;
                            }

                            text.text = image.arguments[3];

                            //设置字体,注意unity中的字体名需要和导出的xml中的一致
                            string fontFolder;

                            if (image.arguments[1].ToLower().Contains("static"))
                            {
                                fontFolder = PSDImporterConst.FONT_STATIC_FOLDER;
                            }
                            else
                            {
                                fontFolder = fontFolder = PSDImporterConst.FONT_FOLDER;
                            }
                            string fontFullName = fontFolder + image.arguments[1] + PSDImporterConst.FONT_SUFIX;
                            Debug.Log("font name ; " + fontFullName);
                            var font = AssetDatabase.LoadAssetAtPath(fontFullName, typeof(Font)) as Font;
                            if (font == null)
                            {
                                Debug.LogWarning("Load font failed : " + fontFullName);
                            }
                            else
                            {
                                text.font = font;
                            }
                            //ps的size在unity里面太小，文本会显示不出来,暂时选择溢出
                            text.verticalOverflow = VerticalWrapMode.Overflow;
                            text.horizontalOverflow = HorizontalWrapMode.Overflow;
                            //设置对齐
                            if (image.arguments.Length >= 5)
                                text.alignment = ParseAlignmentPS2UGUI(image.arguments[4]);
                            else
                            {
                                text.alignment = TextAnchor.MiddleLeft;
                            }*/
                        }
                    }
                    else
                    {
                        if (image.name.ToLower().Contains("background"))
                        {
                            if (image.imageSource == ImageSource.Common || image.imageSource == ImageSource.Custom)
                            {
                                string assetPath = PSDImportUtility.baseDirectory + image.name + PSDImporterConst.PNG_SUFFIX;
                                Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;
                                inputfield.image.sprite = sprite;

                                RectTransform rectTransform = inputfield.GetComponent<RectTransform>();
                                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
                            }
                            else if(image.imageSource == ImageSource.Global)
                            {
                                SpriteImport.SetGlobalImage(inputfield.image, image);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ps的对齐转换到ugui，暂时只做水平的对齐
        /// </summary>
        /// <param name="justification"></param>
        /// <returns></returns>
        public TextAnchor ParseAlignmentPS2UGUI(string justification)
        {
            var defaut = TextAnchor.MiddleCenter;
            if (string.IsNullOrEmpty(justification))
            {
                return defaut;
            }

            string[] temp = justification.Split('.');
            if (temp.Length != 2)
            {
                Debug.LogWarning("ps exported justification is error !");
                return defaut;
            }
            Justification justi = (Justification)System.Enum.Parse(typeof(Justification), temp[1]);
            int index = (int)justi;
            defaut = (TextAnchor)System.Enum.ToObject(typeof(TextAnchor), index); ;

            return defaut;
        }

        //ps的对齐方式
        public enum Justification
        {
            CENTERJUSTIFIED = 0,
            LEFTJUSTIFIED = 1,
            RIGHTJUSTIFIED = 2,
            LEFT = 3,
            CENTER = 4,
            RIGHT = 5,
            FULLYJUSTIFIED = 6,
        }
    }
}