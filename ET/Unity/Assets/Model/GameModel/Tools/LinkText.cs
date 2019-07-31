using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.EventSystems;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


//标签类型
public enum RichTextTagType { None, Underline, }

//标签基类
public abstract class RichTextTag
{
    public int start;
    public int end;

    public abstract RichTextTagType tagType { get; }

    public abstract void SetValue(string key, string value);
}

//下划线标签
public class RichTextUnderlineTag : RichTextTag
{
    public Color color = Color.white;//颜色
    public float height = 1f;//高度
    public string eventName;//事件名
    public string eventParameter;//事件参数

    public override RichTextTagType tagType { get { return RichTextTagType.Underline; } }

    public override void SetValue(string key, string value)
    {
        switch (key)
        {
            case "c":
                {
                    ColorUtility.TryParseHtmlString(value, out color);
                    break;
                }
            case "h":
                {
                    float.TryParse(value, out height);
                    break;
                }
            case "n":
                {
                    eventName = value;
                    break;
                }
            case "p":
                {
                    eventParameter = value;
                    break;
                }
            default:
                break;
        }
    }
}

//解析一条标签：<material xxx>
public class RichTextTagParser
{
    private static readonly Regex tagRegex = new Regex(@"<material=([^>\s]+)([^>]*)>");//(标签类型)(标签参数)
    private static readonly Regex paraRegex = new Regex(@"(\w+)=([^\s]+)");//(key)=(value)
    public string content;//<material=xxx xxx>
    public int start;
    public int end;

    public RichTextTag Parse()
    {
        RichTextTag tag = null;
        Match match = tagRegex.Match(content);
        if (match.Success)
        {
            string tagName = match.Groups[1].Value;//标签类型
            if (!tagName.StartsWith("#"))
            {
                var keyValueCollection = paraRegex.Matches(match.Groups[2].Value);//标签参数
                switch (tagName)
                {
                    case "underline":
                        {
                            tag = new RichTextUnderlineTag();
                            break;
                        }
                    default:
                        break;
                }
                if (tag != null)
                {
                    tag.start = start;
                    tag.end = end;
                    for (int i = 0; i < keyValueCollection.Count; i++)
                    {
                        string key = keyValueCollection[i].Groups[1].Value;
                        string value = keyValueCollection[i].Groups[2].Value;
                        tag.SetValue(key, value);
                    }
                }
            }
        }
        return tag;
    }
}

//解析全部标签
public class RichTextParser
{
    private static readonly Regex regex = new Regex(@"</*material[^>]*>");//<material xxx> or </material>
    private const string endStr = "</material>";

    private Stack<RichTextTagParser> tagParserStack;
    private List<RichTextTag> tagList;

    public RichTextParser()
    {
        tagParserStack = new Stack<RichTextTagParser>();
        tagList = new List<RichTextTag>();
    }

    public void Parse(string richText, out List<RichTextTag> tags)
    {
        tagParserStack.Clear();
        tagList.Clear();
        Match match = regex.Match(richText);
        while (match.Success)
        {
            if (match.Value == endStr)
            {
                if (tagParserStack.Count > 0)
                {
                    RichTextTagParser tagParser = tagParserStack.Pop();
                    tagParser.end = match.Index - 1;
                    if (tagParser.end >= tagParser.start)
                    {
                        RichTextTag tag = tagParser.Parse();
                        if (tag != null)
                        {
                            tagList.Add(tag);
                        }
                    }
                }
            }
            else
            {
                RichTextTagParser tagParser = new RichTextTagParser();
                tagParser.content = match.Value;
                tagParser.start = match.Index + match.Length;
                tagParserStack.Push(tagParser);
            }
            match = match.NextMatch();
        }
        tags = tagList;
    }
}

public class RichTextImageInfo
{
    public string name;       //名字(路径)
    public Vector2 size;      //宽高
    public Vector2 position;  //位置
    public int startVertex;   //起始顶点
    public int vertexLength;  //占据顶点数
    public Color color;       //颜色

    //标签属性
    public float widthScale = 1f;              //宽度缩放
    public float heightScale = 1f;             //高度缩放
    public string eventName;                   //事件名
    public string eventParameter;              //事件参数
    public int count = 0;                      //帧数

    public void SetValue(string key, string value)
    {
        switch (key)
        {
            case "w":
                {
                    float.TryParse(value, out widthScale);
                    break;
                }
            case "h":
                {
                    float.TryParse(value, out heightScale);
                    break;
                }
            case "n":
                {
                    eventName = value;
                    break;
                }
            case "p":
                {
                    eventParameter = value;
                    break;
                }
            case "c":
                {
                    int.TryParse(value, out count);
                    break;
                }
            default:
                break;
        }
    }
}

public class RichTextEvent
{
    public Rect rect;//触发事件的判定区域
    public string name;//事件名
    public string parameter;//事件参数
}


//下划线<material=underline c=#ffffff h=1 n=*** p=***>blablabla...</material>
//图片<icon name=*** w=1 h=1 n=*** p=*** c=1/>
//动态表情：<icon name=num_ w=1 h=1 n=*** p=*** c=3/>，对应图片num_1，num_2，num_3
public class LinkText : Text, IPointerClickHandler
{
    private FontData fontData = FontData.defaultFontData;

    //--------------------------------------------------------图片 start
    private static readonly string replaceStr = "\u00A0";
    private static readonly Regex imageTagRegex = new Regex(@"<icon name=([^>\s]+)([^>]*)/>");//(名字)(属性)
    private static readonly Regex imageParaRegex = new Regex(@"(\w+)=([^\s]+)");//(key)=(value)
    private List<RichTextImageInfo> imageInfoList = new List<RichTextImageInfo>();
    private bool isImageDirty = false;
    //--------------------------------------------------------图片 end

    //--------------------------------------------------------文字 start
    private RichTextParser richTextParser = new RichTextParser();
    //--------------------------------------------------------文字 end

    //--------------------------------------------------------事件 start
    private Action<string, string> clickAction;
    private List<RichTextEvent> eventList = new List<RichTextEvent>();
    //--------------------------------------------------------事件 end

    //--------------------------------------------------------调试 start
    private bool isShowClickArea = false;//是否显示可点击区域
    //--------------------------------------------------------调试 end

    protected LinkText()
    {
        fontData = typeof(Text).GetField("m_FontData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this) as FontData;
    }

    readonly UIVertex[] m_TempVerts = new UIVertex[4];

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if (font == null)
            return;

        // We don't care if we the font Texture changes while we are doing our Update.
        // The end result of cachedTextGenerator will be valid for this instance.
        // Otherwise we can get issues like Case 619238.
        m_DisableFontTextureRebuiltCallback = true;

        //处理事件
        eventList.Clear();

        //处理图片标签
        string richText = text;
        IList<UIVertex> verts = null;
        richText = CalculateLayoutWithImage(richText, out verts);

        //处理文字标签
        List<RichTextTag> tagList = null;
        richTextParser.Parse(richText, out tagList);
        for (int i = 0; i < tagList.Count; i++)
        {
            RichTextTag tag = tagList[i];
            switch (tag.tagType)
            {
                case RichTextTagType.None:
                    break;
                case RichTextTagType.Underline:
                    ApplyUnderlineEffect(tag as RichTextUnderlineTag, verts);
                    break;
                default:
                    break;
            }
        }

        Rect inputRect = rectTransform.rect;

        // get the text alignment anchor point for the text in local space
        Vector2 textAnchorPivot = GetTextAnchorPivot(fontData.alignment);
        Vector2 refPoint = Vector2.zero;
        refPoint.x = Mathf.Lerp(inputRect.xMin, inputRect.xMax, textAnchorPivot.x);
        refPoint.y = Mathf.Lerp(inputRect.yMin, inputRect.yMax, textAnchorPivot.y);

        // Determine fraction of pixel to offset text mesh.
        Vector2 roundingOffset = PixelAdjustPoint(refPoint) - refPoint;

        // Apply the offset to the vertices
        //IList<UIVertex> verts = cachedTextGenerator.verts;
        float unitsPerPixel = 1 / pixelsPerUnit;
        //Last 4 verts are always a new line...
        int vertCount = verts.Count - 4;

        toFill.Clear();
        if (roundingOffset != Vector2.zero)
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                m_TempVerts[tempVertsIndex] = verts[i];
                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(m_TempVerts);
            }
        }
        else
        {
            //Debug.Log(unitsPerPixel);
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                m_TempVerts[tempVertsIndex] = verts[i];
                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(m_TempVerts);
                //Debug.LogWarning(i + "_" + tempVertsIndex + "_" + m_TempVerts[tempVertsIndex].position);
            }
        }
        m_DisableFontTextureRebuiltCallback = false;
    }

    protected string CalculateLayoutWithImage(string richText, out IList<UIVertex> verts)
    {
        Vector2 extents = rectTransform.rect.size;
        var settings = GetGenerationSettings(extents);

        float unitsPerPixel = 1 / pixelsPerUnit;

        float spaceWidth = cachedTextGenerator.GetPreferredWidth(replaceStr, settings) * unitsPerPixel;

        float fontSize2 = fontSize * 0.5f;

        //解析图片标签，并将标签替换为空格
        imageInfoList.Clear();
        Match match = null;
        StringBuilder builder = new StringBuilder();
        while ((match = imageTagRegex.Match(richText)).Success)
        {
            RichTextImageInfo imageInfo = new RichTextImageInfo();
            imageInfo.name = match.Groups[1].Value;
            string paras = match.Groups[2].Value;
            if (!string.IsNullOrEmpty(paras))
            {
                var keyValueCollection = imageParaRegex.Matches(paras);
                for (int i = 0; i < keyValueCollection.Count; i++)
                {
                    string key = keyValueCollection[i].Groups[1].Value;
                    string value = keyValueCollection[i].Groups[2].Value;
                    imageInfo.SetValue(key, value);
                }
            }
            imageInfo.size = new Vector2(fontSize2 * imageInfo.widthScale, fontSize2 * imageInfo.heightScale);
            imageInfo.startVertex = match.Index * 4;
            int num = Mathf.CeilToInt(imageInfo.size.x / spaceWidth);//占据几个空格
            imageInfo.vertexLength = num * 4;
            imageInfoList.Add(imageInfo);

            builder.Length = 0;
            builder.Append(richText, 0, match.Index);
            for (int i = 0; i < num; i++)
            {
                builder.Append(replaceStr);
            }
            builder.Append(richText, match.Index + match.Length, richText.Length - match.Index - match.Length);
            richText = builder.ToString();
        }

        // Populate charaters
        cachedTextGenerator.Populate(richText, settings);
        verts = cachedTextGenerator.verts;
        // Last 4 verts are always a new line...
        int vertCount = verts.Count - 4;

        //换行处理
        //0 1|4 5|8  9
        //3 2|7 6|11 10
        //例如前两个字为图片标签，第三字为普通文字；那么startVertex为0，vertexLength为8
        for (int i = 0; i < imageInfoList.Count; i++)
        {
            RichTextImageInfo imageInfo = imageInfoList[i];
            int startVertex = imageInfo.startVertex;
            int vertexLength = imageInfo.vertexLength;
            int maxVertex = Mathf.Min(startVertex + vertexLength, vertCount);
            //如果最边缘顶点超过了显示范围，则将图片移到下一行
            //之后的图片信息中的起始顶点都往后移
            if (verts[maxVertex - 2].position.x * unitsPerPixel > rectTransform.rect.xMax) //所有空格在同一行
            {
                richText = richText.Insert(startVertex / 4, "\r\n");
                for (int j = i; j < imageInfoList.Count; j++)
                {
                    imageInfoList[j].startVertex += 8;
                }
                cachedTextGenerator.Populate(richText, settings);
                verts = cachedTextGenerator.verts;
                vertCount = verts.Count - 4;
            }
            else //空格不在同一行
            {
                float lastX = verts[startVertex].position.x;

                for (int j = startVertex + 4; j < startVertex + vertexLength; j += 4)
                {
                    if (verts[j].position.x < lastX)
                    {
                        richText = richText.Insert(startVertex / 4, "\r\n");
                        for (int k = i; k < imageInfoList.Count; k++)
                        {
                            imageInfoList[k].startVertex += 8;
                        }
                        cachedTextGenerator.Populate(richText, settings);
                        verts = cachedTextGenerator.verts;
                        vertCount = verts.Count - 4;
                        break;
                    }
                    else
                    {
                        lastX = verts[j].position.x;
                    }
                }
            }
        }

        //计算位置
        for (int i = imageInfoList.Count - 1; i >= 0; i--)
        {
            RichTextImageInfo imageInfo = imageInfoList[i];
            int startVertex = imageInfo.startVertex;
            if (startVertex < vertCount)
            {
                UIVertex uiVertex = verts[startVertex];
                Vector2 pos = uiVertex.position;
                pos *= unitsPerPixel;
                pos += new Vector2(imageInfo.size.x * 0.5f, fontSize2 * 0.5f);
                pos += new Vector2(rectTransform.sizeDelta.x * (rectTransform.pivot.x - 0.5f), rectTransform.sizeDelta.y * (rectTransform.pivot.y - 0.5f));
                imageInfo.position = pos;
                imageInfo.color = Color.white;

                if (!string.IsNullOrEmpty(imageInfo.eventName))
                {
                    //图片pos：
                    //x:起点x + 图片宽度的一半
                    //y:起点y + fontSize2 * 0.5f
                    RichTextEvent e = new RichTextEvent();
                    e.name = imageInfo.eventName;
                    e.parameter = imageInfo.eventParameter;
                    e.rect = new Rect(
                        verts[startVertex].position.x * unitsPerPixel,
                        verts[startVertex].position.y * unitsPerPixel + fontSize2 * 0.5f - imageInfo.size.y * 0.5f,
                        imageInfo.size.x,
                        imageInfo.size.y
                    );
                    eventList.Add(e);
                }
            }
            else
            {
                imageInfoList.RemoveAt(i);
            }
        }

        isImageDirty = true;

        return richText;
    }

    private void ApplyUnderlineEffect(RichTextUnderlineTag tag, IList<UIVertex> verts)
    {
        float fontSize2 = fontSize * 0.5f;
        float unitsPerPixel = 1 / pixelsPerUnit;

        //0 1|4 5|8  9 |12 13
        //3 2|7 6|11 10|14 15
        //<material=underline c=#ffffff h=1 n=1 p=2>下划线</material>
        //以上面为例:
        //tag.start为42，对应“>” | start对应“下”的左上角顶点
        //tag.end为44，对应“划”  | end对应“线”下一个字符的左上角顶点
        //Debug.Log(tag.start);
        //Debug.Log(tag.end);
        int start = tag.start * 4;
        int end = Mathf.Min(tag.end * 4 + 4, verts.Count);
        UIVertex vt1 = verts[start + 3];
        UIVertex vt2;
        float minY = vt1.position.y;
        float maxY = verts[start].position.y;

        //换行处理，如需换行，则将一条下划线分割成几条
        //顶点取样分布，如上图的2，6，10，其中end - 2表示最后一个取样点，即10
        //对应例子中的下、划、线的右下角顶点
        for (int i = start + 2; i <= end - 2; i += 4)
        {
            vt2 = verts[i];
            bool newline = Mathf.Abs(vt2.position.y - vt1.position.y) > fontSize2;
            if (newline || i == end - 2)
            {
                RichTextImageInfo imageInfo = new RichTextImageInfo();

                //计算宽高
                int tailIndex = !newline && i == end - 2 ? i : i - 4;
                vt2 = verts[tailIndex];
                minY = Mathf.Min(minY, vt2.position.y);
                maxY = Mathf.Max(maxY, verts[tailIndex - 1].position.y);
                imageInfo.size = new Vector2((vt2.position.x - vt1.position.x) * unitsPerPixel, tag.height);

                //计算位置
                Vector2 vertex = new Vector2(vt1.position.x, minY);
                vertex *= unitsPerPixel;
                vertex += new Vector2(imageInfo.size.x * 0.5f, -tag.height * 0.5f);
                vertex += new Vector2(rectTransform.sizeDelta.x * (rectTransform.pivot.x - 0.5f), rectTransform.sizeDelta.y * (rectTransform.pivot.y - 0.5f));
                imageInfo.position = vertex;

                imageInfo.color = tag.color;
                imageInfoList.Add(imageInfo);

                if (!string.IsNullOrEmpty(tag.eventName))
                {
                    //下划线pos：
                    //x:vt1.x + 图片宽度的一半
                    //y:minY - tag.height * 0.5f
                    RichTextEvent e = new RichTextEvent();
                    e.name = tag.eventName;
                    e.parameter = tag.eventParameter;
                    e.rect = new Rect(
                        vt1.position.x * unitsPerPixel,
                        minY * unitsPerPixel,
                        imageInfo.size.x,
                        (maxY - minY) * unitsPerPixel
                    );
                    eventList.Add(e);
                }

                vt1 = verts[i + 1];
                minY = vt1.position.y;
                if (newline && i == end - 2) i -= 4;
            }
            else
            {
                minY = Mathf.Min(minY, verts[i].position.y);
                maxY = Mathf.Max(maxY, verts[i - 1].position.y);
            }
        }
    }

    public void SetListener(Action<string, string> action) { clickAction = action; }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPos);
        for (int i = eventList.Count - 1; i >= 0; i--)
        {
            RichTextEvent e = eventList[i];
            if (e.rect.Contains(localPos))
            {
                clickAction.Invoke(e.name, e.parameter);
                break;
            }
        }
    }

    protected void Update()
    {
        if (isImageDirty)
        {
            isImageDirty = false;

            //回收资源
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                DestroyImmediate(child);
                i--;
            }

            //生成图片或帧动画
            for (int i = 0; i < imageInfoList.Count; i++)
            {
                RichTextImageInfo imageInfo = imageInfoList[i];
                var name = imageInfo.name;
                var position = imageInfo.position;
                var size = imageInfo.size;
                var color = imageInfo.color;

                GameObject go = null;
                Image image = null;

                if (imageInfo.count == 0)
                {
                    go = new GameObject("underline");
                    image = go.AddComponent<Image>();
                }
                else
                {

                }
                go.layer = LayerMask.NameToLayer("UI");
                go.transform.SetParent(rectTransform);
                go.transform.localScale = Vector3.one;
                image.rectTransform.anchoredPosition = position;
                image.rectTransform.localPosition = new Vector3(image.rectTransform.localPosition.x, image.rectTransform.localPosition.y, 0);
                image.rectTransform.sizeDelta = size;
                image.color = color;
            }


        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        //Debug.LogWarning("OnDestroy");
    }
}