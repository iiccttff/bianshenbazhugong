//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// This script makes it possible for a scroll view to wrap its content, creating endless scroll views.
/// Usage: simply attach this script underneath your scroll view where you would normally place a UIGrid:
/// 
/// + ScrollView(PanelMove)
/// |- PUIWrapContent(Items)
/// |-- Item 1
/// |-- Item 2
/// |-- Item 3
/// </summary>

[AddComponentMenu("Custom/PUIWrapContent")]


public enum WrapType
{
    RightSide = 1,
    LeftSide,
    UpSide,
}
public class PUIWrapContent : MonoBehaviour
{
    public static PUIWrapContent Get(GameObject go)
    {
        PUIWrapContent wrap = go.GetComponent<PUIWrapContent>();
        if (wrap == null) wrap = go.AddComponent<PUIWrapContent>();
        return wrap;
    }

	/// <summary>列表成员尺寸</summary>
	public Vector2 itemSize = new Vector2(100.0f, 100.0f);

	/// <summary>超出UIPanel是否激死</summary>
	public bool cullContent = false;

    /// <summary>列表第一个成员的定位点</summary>
    public float first = 0.0f;
    /// <summary>列表首序号</summary>
    public int from = 0;
    /// <summary>列表尾序号</summary>
    public int to = 9;
    /// <summary>列表总成员个数</summary>
    public int maxNum = 100;

    /// <summary>列表首标志</summary>
    public UISprite spriteStart;
    /// <summary>列表尾标志</summary>
    public UISprite spriteEnd;
    /// <summary>列表首之前组</summary>
    public Transform groupBeforeStart;
    /// <summary>列表尾之后组</summary>
    public Transform groupAfterEnd;

    /// <summary>自动居中</summary>
    public bool autoCenter = false;
    /// <summary>启动复位列表</summary>
    public bool resetOnStart = false;
    /// <summary>灵敏度</summary>
    private float momentumAmount = 60.0f;
    /// <summary>是否循环列表</summary>
    public bool wrapContent = true;
    public bool disableDragIfFits = false;

    public delegate void Callback(GameObject item, int index);
    /// <summary>更新列表成员数据回调接口</summary>
    public Callback updateItemCallback = null;
    /// <summary>列表成员飞入完成回调接口</summary>
    public Callback moveEffectFinishCallback = null;
    /// <summary>列表成员点击事件回调接口</summary>
    public UIEventListener.VoidDelegate clickCallback = null;
    /// <summary>列表成员自动居中回调接口</summary>
    public UIEventListener.VoidDelegate centerCallback = null;

	private Transform mTrans;
    private UIScrollView mScroll;
    private bool mHorizontal = false;
    private List<Transform> mChildren = new List<Transform>();

    private bool mStopYieldFlag = false;
    private bool mResetOverFlag = true;


    public WrapType WrapContentType = WrapType.RightSide;//;
    void Start()
    {
        if (resetOnStart == true)
        {
            Reset(true);
        }
    }

    #region 复位处理
    public void Reset(bool moveEffect)
    {
        if(this.gameObject.activeInHierarchy)
           StartCoroutine(DelayReset(moveEffect));
    }

    IEnumerator DelayReset(bool moveEffect)
    {
        mResetOverFlag = false;
        yield return 1;
        if (mStopYieldFlag == true) { yield break; }
        SortBasedOnScrollMovement(moveEffect);
        if (mScroll != null && mScroll.panel != null)
        {
            SpringPanel spring = mScroll.panel.GetComponent<SpringPanel>();
            if (spring != null)
            {
                Destroy(spring);
            }
            if (mInitPanelOffset == false)
            {
                mPanelClipOffset = mScroll.panel.clipOffset;
                mPanleLocalPosition = mScroll.panel.transform.localPosition;
                mInitPanelOffset = true;
            }
            ResetPanelClipOffset();
            mScroll.UpdateScrollbars(true);
            //mScroll.onPressUp = OnPressUp;
            mScroll.panel.onClipMove = OnMove;
            mScroll.disableDragIfFits = disableDragIfFits;
            if (autoCenter)
            {
                mScroll.momentumAmount = momentumAmount * 0.7f;
            }
            else
            {
                mScroll.momentumAmount = momentumAmount;
            }
        }

        mResetOverFlag = true;
    }

    private Vector2 mPanelClipOffset = Vector2.zero;
    private Vector3 mPanleLocalPosition = Vector3.zero;
    private bool mInitPanelOffset = false;

    private void ResetPanelClipOffset()
    {
        if (mScroll != null && mScroll.panel != null)
        {
            float baseFirst = first;
            if (groupBeforeStart != null && baseFirst <= 0.0f)
            {
                baseFirst -= 1.0f;
            }
            if (autoCenter == true)
            {
                if (baseFirst >= 0.0f)
                {
                    baseFirst = (int)baseFirst;
                }
                else
                {
                    baseFirst = -1.0f;
                }
            }
            float modifyOffset = 0.0f;
            if (mHorizontal)
            {
                if (autoCenter == true)
                {
                    modifyOffset = itemSize.x / 2.0f - mScroll.panel.finalClipRegion.z / 2.0f;
                }
                mScroll.panel.clipOffset = mPanelClipOffset + new Vector2(itemSize.x, 0.0f) * baseFirst + new Vector2(modifyOffset,0.0f);
                mScroll.panel.transform.localPosition = mPanleLocalPosition - new Vector3(itemSize.x, 0.0f, 0.0f) * baseFirst - new Vector3(modifyOffset,0.0f,0.0f);
            }
            else
            {
                if (autoCenter == true)
                {
                    modifyOffset = itemSize.y / 2.0f - mScroll.panel.finalClipRegion.w / 2.0f;
                }
                mScroll.panel.clipOffset = mPanelClipOffset - new Vector2(0.0f, itemSize.y) * baseFirst - new Vector2(0.0f, modifyOffset);
                mScroll.panel.transform.localPosition = mPanleLocalPosition + new Vector3(0.0f, itemSize.y, 0.0f) * baseFirst + new Vector3(0.0f,modifyOffset, 0.0f);
            }
            //mScroll.panel.Refresh();
        }
    }
    #endregion

    #region 自动居中处理
    void OnPressUp()
    {
        if (autoCenter)
        {
            ReCenter();
        }
    }

    private float nextPageThreshold = 0f;
    private GameObject mCenteredObject;

    [ContextMenu("ReCenter")]
    public void ReCenter()
    {
        if (mScroll != null && mScroll.panel != null)
        {
            if (maxNum == 0)
            {
                return;
            }
            // Calculate the panel's center in world coordinates
            Vector3[] corners = mScroll.panel.worldCorners;
            Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

            // Offset this value by the momentum
            Vector3 momentum = mScroll.currentMomentum * mScroll.momentumAmount;
            Vector3 moveDelta = NGUIMath.SpringDampen(ref momentum, 9f, 2f);
            Vector3 pickingPoint = panelCenter - moveDelta * 0.05f; // Magic number based on what "feels right"
            mScroll.currentMomentum = Vector3.zero;

            float min = float.MaxValue;
            Transform closest = null;
            int index = 0;

            // Determine the closest child
            for (int i = 0; i < mChildren.Count; ++i)
		    {
			    Transform t = mChildren[i];
                if (!t.gameObject.activeInHierarchy) continue;
                float sqrDist = Vector3.SqrMagnitude(t.position - pickingPoint);

                if (sqrDist < min)
                {
                    min = sqrDist;
                    closest = t;
                    index = i;
                }
            }

            // If we have a touch in progress and the next page threshold set
            if (nextPageThreshold > 0f && UICamera.currentTouch != null)
            {
                // If we're still on the same object
                if (mCenteredObject != null && mCenteredObject.transform == mChildren[index])
                {
                    Vector2 totalDelta = UICamera.currentTouch.totalDelta;

                    float delta = 0f;

                    switch (mScroll.movement)
                    {
                        case UIScrollView.Movement.Horizontal:
                            {
                                delta = totalDelta.x;
                                break;
                            }
                        case UIScrollView.Movement.Vertical:
                            {
                                delta = totalDelta.y;
                                break;
                            }
                        default:
                            {
                                delta = totalDelta.magnitude;
                                break;
                            }
                    }

                    if (delta > nextPageThreshold)
                    {
                        // Next page
                        if (index > 0)
                            closest = mChildren[index-1];
                    }
                    else if (delta < -nextPageThreshold)
                    {
                        // Previous page
                        if (index < mChildren.Count - 1)
                            closest = mChildren[index+1];
                    }
                }
            }

            CenterOn(closest, panelCenter);
        }
    }
    /// <summary>
    /// Center the panel on the specified target.
    /// </summary>
    void CenterOn(Transform target, Vector3 panelCenter)
    {
        if (target != null && mScroll != null && mScroll.panel != null)
        {
            Transform panelTrans = mScroll.panel.cachedTransform;
            mCenteredObject = target.gameObject;
            // Figure out the difference between the chosen child and the panel's center in local coordinates
            Vector3 cp = panelTrans.InverseTransformPoint(target.position);
            Vector3 cc = panelTrans.InverseTransformPoint(panelCenter);
            Vector3 localOffset = cp - cc;

            // Offset shouldn't occur if blocked
            if (!mScroll.canMoveHorizontally) localOffset.x = 0f;
            if (!mScroll.canMoveVertically) localOffset.y = 0f;
            localOffset.z = 0f;

            // Spring the panel to this calculated position
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                panelTrans.localPosition = panelTrans.localPosition - localOffset;

                Vector4 co = mScroll.panel.clipOffset;
                co.x += localOffset.x;
                co.y += localOffset.y;
                mScroll.panel.clipOffset = co;
            }
            else
#endif
            {
                SpringPanel.Begin(mScroll.panel.cachedGameObject, panelTrans.localPosition - localOffset, 10);//.onFinished = onFinished
            }

            // Notify the listener
            if (centerCallback != null)
            {
                centerCallback(mCenteredObject);
            }
        }
        else
        {
            mCenteredObject = null;
        }
    }
    #endregion

	/// <summary>
	/// Callback triggered by the UIPanel when its clipping region moves (for example when it's being scrolled).
	/// </summary>
	protected virtual void OnMove (UIPanel panel) 
    { 
        if (mResetOverFlag == true)
        {
            if (wrapContent)
            {
                WrapContent();
            }
        }
    }

	/// <summary>
	/// Immediately reposition all children.
	/// </summary>
	[ContextMenu("Sort Based on Scroll Movement")]
    public void SortBasedOnScrollMovement(bool moveEffect)
	{
		if (!CacheScrollView()) return;

        float maxFirst = from;
        int needNum = maxNum;
        if (wrapContent)
        {
            if (mHorizontal)
            {
                maxFirst = maxNum - mScroll.panel.finalClipRegion.z / itemSize.x;
                needNum = (int)(mScroll.panel.finalClipRegion.z / itemSize.x) + 2;
            }
            else
            {
                maxFirst = maxNum - mScroll.panel.finalClipRegion.z / itemSize.x;
                needNum = (int)(mScroll.panel.finalClipRegion.w / itemSize.y) + 2;
            }
            if (first < 0.0f)
            {
                first = 0.0f;
            }
            else if (first > maxFirst && maxFirst > 0.0f)
            {
                first = maxFirst;
            }
            from = (int)first;
            if (from > 0)
            {
                from -= 1;
            }
            to = from + needNum - 1;
            if (to > maxNum - 1)
            {
                to = maxNum - 1;
                from = to - needNum + 1;
                if (from < 0)
                {
                    from = 0;
                }
            }
        }
        else
        {
            to = maxNum - 1;
        }

        if (to < 0)
        {
            to = 0;
        }

        if (mTrans.childCount < needNum)
        {
            if (mTrans.childCount > 0)
            {
                mTrans.GetChild(0).gameObject.SetActive(true);
                Transform item = mTrans.GetChild(0);
                int max = needNum - mTrans.childCount;
                for (int i = 0; i < max; ++i)
                {
                    NGUITools.AddChild(this.gameObject, item.gameObject);
                }
            }
        }

		// Cache all children and place them in order
		mChildren.Clear();
        if (mTrans.childCount > 0)
        {
            for (int i = 0; i < mTrans.childCount; ++i)
            {
                Transform tran = mTrans.GetChild(i);

                List<BoxCollider> boxs = GetComponentsAll<BoxCollider>(tran.gameObject, true);
                if (boxs != null && boxs.Count > 0)
                {
                    for (int j = 0; j < boxs.Count; j++)
                    {
                        BoxCollider box = boxs[j];
                        if (clickCallback != null)
                        {
                            UIEventListener.Get(box.gameObject).onClick = clickCallback;
                        }
                        UIDragScrollView drag = box.GetComponent<UIDragScrollView>();
                        if (drag == null)
                        {
                            drag = box.gameObject.AddComponent<UIDragScrollView>();
                        }
                        if (drag.scrollView == null)
                        {
                            drag.scrollView = mScroll;
                        }
                    }
                }
                mChildren.Add(tran);
            }
        }

        if (mChildren.Count > 0)
        {
            // Sort the list of children so that they are in order
            if (mHorizontal) mChildren.Sort(UIGrid.SortHorizontal);
            else mChildren.Sort(UIGrid.SortVertical);
        }
        
		ResetChildPositions(moveEffect);
	}


    /// <summary>获取对象上及子对象上所有的组件</summary>
    private List<T> GetComponentsAll<T>(GameObject go, bool includeInactive) where T : Component
    {
        List<T> listT = new List<T>();
        if (go != null)
        {
            T t = go.GetComponent<T>();
            if (t != null)
            {
                listT.Add(t);
            }
            T[] ts = go.GetComponentsInChildren<T>(includeInactive);
            if (ts != null)
            {
                for (int i = 0, max = ts.Length; i < max; i++)
                {
                    T tt = ts[i];
                    listT.Add(tt);
                }
            }
        }
        return listT;
    }

	/// <summary>
	/// Immediately reposition all children, sorting them alphabetically.
	/// </summary>
	[ContextMenu("Sort Alphabetically")]
	public void SortAlphabetically ()
	{
		if (!CacheScrollView()) return;

		// Cache all children and place them in order
		mChildren.Clear();
		for (int i = 0; i < mTrans.childCount; ++i)
			mChildren.Add(mTrans.GetChild(i));

		// Sort the list of children so that they are in order
		mChildren.Sort(UIGrid.SortByName);
		ResetChildPositions(false);
	}

	/// <summary>
	/// Cache the scroll view and return 'false' if the scroll view is not found.
	/// </summary>
	protected bool CacheScrollView ()
	{
        mTrans = transform;
        mTrans.GetChild(0).gameObject.SetActive(false);
        mScroll = NGUITools.FindInParents<UIScrollView>(gameObject);

        if (mScroll == null) return false;
        if (mScroll.movement == UIScrollView.Movement.Horizontal) mHorizontal = true;
        else if (mScroll.movement == UIScrollView.Movement.Vertical) mHorizontal = false;
        else return false;

        return true;
	}

    private Vector3 mClipTopLeftPos = Vector3.zero;

	/// <summary>
	/// Helper function that resets the position of all the children.
	/// </summary>
	void ResetChildPositions (bool moveEffect)
	{
        mClipTopLeftPos = new Vector3(-mScroll.panel.finalClipRegion.z / 2.0f + mScroll.panel.clipSoftness.x, mScroll.panel.finalClipRegion.w / 2.0f - mScroll.panel.clipSoftness.y, 0.0f);
        mListSpringPosition.Clear();
        //---
        if (spriteStart != null)
        {
            spriteStart.width = (int)itemSize.x;
            spriteStart.height = (int)itemSize.y;
            //spriteStart.color = new Color(0.0f, 0.0f, 0.0f, 1.0f / 255.0f);
            spriteStart.transform.localPosition = mHorizontal ? new Vector3(mClipTopLeftPos.x + itemSize.x / 2.0f, 0f, 0f) : new Vector3(0f, mClipTopLeftPos.y - itemSize.y / 2.0f, 0f);
            if (spriteStart.gameObject.activeSelf == false)
            {
                spriteStart.gameObject.SetActive(true);
            }
        }
        if (spriteEnd != null)
        {
            int index = maxNum - 1;
            if (index < 0)
            {
                index = 0;
            }
            spriteEnd.width = (int)itemSize.x;
            spriteEnd.height = (int)itemSize.y;
            //spriteEnd.color = new Color(0.0f, 0.0f, 0.0f, 1.0f / 255.0f);
            spriteEnd.transform.localPosition = mHorizontal ? new Vector3(mClipTopLeftPos.x + itemSize.x / 2.0f + (float)(index) * itemSize.x, 0f, 0f) : new Vector3(0f, mClipTopLeftPos.y - itemSize.y / 2.0f - (float)(index) * itemSize.y, 0f);
            if (spriteEnd.gameObject.activeSelf == false)
            {
                spriteEnd.gameObject.SetActive(true);
            }
        }
        int moveIndex = 1;
        if (groupBeforeStart != null)
        {
            groupBeforeStart.localPosition = mHorizontal ? new Vector3(mClipTopLeftPos.x - itemSize.x / 2.0f, 0f, 0f) : new Vector3(0f, mClipTopLeftPos.y + itemSize.y / 2.0f, 0f);
            if (groupBeforeStart.gameObject.activeSelf == false)
            {
                groupBeforeStart.gameObject.SetActive(true);
            }
            if (moveEffect == true)
            {
                AddMoveEffect(groupBeforeStart.gameObject, moveIndex, false);
                moveIndex++;
            }
        }
        //---
        for (int i = 0; i < mChildren.Count; ++i)
		{
			Transform t = mChildren[i];
            int index = (from + i);
            t.gameObject.name = index.ToString();
            t.localPosition = mHorizontal ? new Vector3(mClipTopLeftPos.x + itemSize.x / 2.0f + index * itemSize.x, 0f, 0f) : new Vector3(0f, mClipTopLeftPos.y - itemSize.y / 2.0f - index * itemSize.y, 0f);
            if (index >= 0 && index < maxNum)
            {
                t.gameObject.SetActive(true);
                if (moveEffect == true)
                {
                    AddMoveEffect(t.gameObject, moveIndex, true);
                    moveIndex++;
                    if (mScroll.enabled == true)
                    {
                        mScroll.enabled = false;
                    }
                }

                UpdateItem(t, index);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
		}
        if (groupAfterEnd != null)
        {
            groupAfterEnd.localPosition = mHorizontal ? new Vector3(mClipTopLeftPos.x + itemSize.x / 2.0f + (float)(maxNum) * itemSize.x, 0f, 0f) : new Vector3(0f, mClipTopLeftPos.y - itemSize.y / 2.0f - (float)(maxNum) * itemSize.y, 0f);
            if (groupAfterEnd.gameObject.activeSelf == false)
            {
                groupAfterEnd.gameObject.SetActive(true);
            }
            if (moveEffect == true)
            {
                AddMoveEffect(groupAfterEnd.gameObject, moveIndex, false);
                moveIndex++;
            }
        }
	}
    void AddMoveEffect(GameObject item, int index, bool addList)
    {
        Vector3 pos = item.transform.localPosition;
        switch (WrapContentType)
        {
            case WrapType.RightSide:
                item.transform.localPosition += new Vector3(mScroll.panel.finalClipRegion.z * (float)(index), 0.0f, 0.0f);
                break;
            case WrapType.UpSide: 
                item.transform.localPosition += new Vector3(0.0f, mScroll.bounds.size.y + mScroll.panel.finalClipRegion.w * (float)(index), 0.0f);
                break;
            case WrapType.LeftSide:
                item.transform.localPosition += new Vector3(-1*mScroll.panel.finalClipRegion.z * (float)(index), 0.0f, 0.0f);
                break;
        }
        PSpringPosition sp = PSpringPosition.Begin(item, pos, 10.0f);
        sp.onFinished = MoveEffectFinishOver;
        if (addList == true)
        {
            mListSpringPosition.Add(sp);
        }
        //Vector3 pos = item.transform.localPosition;
        //
        //PSpringPosition sp = PSpringPosition.Begin(item, pos, 10.0f);
        //sp.onFinished = MoveEffectFinishOver;
        //if (addList == true)
        //{
        //    mListSpringPosition.Add(sp);
        //}
    }
    private List<PSpringPosition> mListSpringPosition = new List<PSpringPosition>();
    void MoveEffectFinishOver(PSpringPosition sp)
    {
        GameObject item = sp.gameObject;
        if (item != null)
        {
            int index = -1;
            int.TryParse(item.name, out index);
            if (index >= 0 && index < maxNum)
            {
                if (moveEffectFinishCallback != null)
                {
                    moveEffectFinishCallback(item, index);
                }
            }
        } 
        mListSpringPosition.Remove(sp);
        if (mListSpringPosition.Count <= 0)
        {
            if (mScroll.enabled == false)
            {
                mScroll.enabled = true;
            }
        }
    }

	/// <summary>
	/// Wrap all content, repositioning all children as needed.
	/// </summary>
	public void WrapContent ()
	{
        Vector3[] corners = mScroll.panel.worldCorners;
		
		for (int i = 0; i < 4; ++i)
		{
			Vector3 v = corners[i];
			v = mTrans.InverseTransformPoint(v);
			corners[i] = v;
		}
		Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);

		if (mHorizontal)
		{
            float extents = itemSize.x * mChildren.Count * 0.5f;
            float ext2 = extents * 2f;
			float min = corners[0].x - itemSize.x;
			float max = corners[2].x + itemSize.x;

            for (int i = 0; i < mChildren.Count; ++i)
			{
				Transform t = mChildren[i];
				float distance = t.localPosition.x - center.x;
                Vector3 pos = t.localPosition;
                bool modify = false;

				if (distance < -extents)
				{
                    pos.x += ext2;
                    distance = pos.x - center.x;
                    modify = true;
				}
				else if (distance > extents)
				{
                    pos.x -= ext2;
                    distance = pos.x - center.x;
                    modify = true;
				}
                if (modify == true)
                {
                    int index = GetIndex(pos);
                    if (index >= 0 && index < maxNum)
                    {
                        if (index < from)
                        {
                            to -= from - index;
                            from = index;
                        }
                        else if (index > to)
                        {
                            from += index - to;
                            to = index;
                        }
                        t.localPosition = pos;
                        UpdateItem(t, index);
                    }
                }
                
				if (cullContent)
				{
                    distance += mScroll.panel.clipOffset.x - mTrans.localPosition.x;
					if (!UICamera.IsPressed(t.gameObject))
						NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
				}
			}
		}
		else
		{
            float extents = itemSize.y * mChildren.Count * 0.5f;
            float ext2 = extents * 2f;
			float min = corners[0].y - itemSize.y;
			float max = corners[2].y + itemSize.y;

            for (int i = 0; i < mChildren.Count; ++i)
			{
				Transform t = mChildren[i];
				float distance = t.localPosition.y - center.y;
                Vector3 pos = t.localPosition;
                bool modify = false;

				if (distance < -extents)
				{
                    pos.y += ext2;
                    distance = pos.y - center.y;
                    modify = true;
				}
				else if (distance > extents)
				{
                    pos.y -= ext2;
                    distance = pos.y - center.y;
                    modify = true;
				}
                
                if (modify == true)
                {
                    int index = GetIndex(pos);
                    if (index >= 0 && index < maxNum)
                    {
                        if (index < from)
                        {
                            to -= from - index;
                            from = index;
                        }
                        else if (index > to)
                        {
                            from += index - to;
                            to = index;
                        }
                        t.localPosition = pos;
                        UpdateItem(t, index);
                    }
                }

				if (cullContent)
				{
                    distance += mScroll.panel.clipOffset.y - mTrans.localPosition.y;
					if (!UICamera.IsPressed(t.gameObject))
						NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
				}
			}
		}
	}

	/// <summary>
	/// Want to update the content of items as they are scrolled? Override this function.
	/// </summary>
	protected virtual void UpdateItem (Transform item, int index)
    {
        if (item != null)
        {
            item.gameObject.name = index.ToString();
            if (index >= 0 && index < maxNum)
            {
                if (item.gameObject.activeSelf == false)
                {
                    item.gameObject.SetActive(true);
                }
                if (updateItemCallback != null)
                {
                    updateItemCallback(item.gameObject, index);
                }
            }
            else
            {
                if (item.gameObject.activeSelf == true)
                {
                    item.gameObject.SetActive(false);
                }
            }
            //Debugger.Log(string.Format("UpdateItem={0}",index.ToString()));
        }
    }
    /// <summary>
    /// 更新滑动控件
    /// </summary>
    private void UpdateScrowView()
    {
        if (spriteEnd != null)
        {
            int index = maxNum - 1;
            if (index < 0)
            {
                index = 0;
            }
            spriteEnd.transform.localPosition = mHorizontal ? new Vector3(mClipTopLeftPos.x + itemSize.x / 2.0f + (float)(index) * itemSize.x, 0f, 0f) : new Vector3(0f, mClipTopLeftPos.y - itemSize.y / 2.0f - (float)(index) * itemSize.y, 0f);
        }
        if (groupAfterEnd != null)
        {
            groupAfterEnd.localPosition = mHorizontal ? new Vector3(mClipTopLeftPos.x + itemSize.x / 2.0f + (float)(maxNum) * itemSize.x, 0f, 0f) : new Vector3(0f, mClipTopLeftPos.y - itemSize.y / 2.0f - (float)(maxNum) * itemSize.y, 0f);
        }
        if (mScroll != null)
        {
            mScroll.UpdateScrollbars(true);
        }
    }
    
    #region 外部接口
    /// <summary>
    /// 获取成员序号
    /// </summary>
    /// <param name="item">成员对象</param>
    /// <returns>序号</returns>
    public int GetIndex(GameObject item)
    {
        int index = -1;
        if (item != null)
        {
            index = GetIndex(item.transform.localPosition);
        }
        return index;
    }
    public int GetIndex(Vector3 pos)
    {
        int index = -1;
        if (mHorizontal)
        {
            if (pos.x > mClipTopLeftPos.x)
            {
                index = (int)((pos.x - mClipTopLeftPos.x) / itemSize.x);
            }
        } 
        else
        {
            if (pos.y < mClipTopLeftPos.y)
            {
                index = (int)((-pos.y + mClipTopLeftPos.y) / itemSize.y);
            }
        }
        return index;
    }
    /// <summary>
    /// 获取指定列表成员坐标
    /// </summary>
    /// <param name="index">指定序号</param>
    /// <returns>列表成员坐标</returns>
    public Vector3 GetItemPosByIndex(int index)
    {
        Vector3 pos = Vector3.zero;
        if (mHorizontal)
        {
            pos = new Vector3(mClipTopLeftPos.x + itemSize.x / 2.0f + (float)index * itemSize.x, 0.0f, 0.0f);
        }
        else
        {
            pos = new Vector3(0.0f, mClipTopLeftPos.y - itemSize.y / 2.0f - (float)index * itemSize.y, 0.0f);
        }
        return pos;
    }
    /// <summary>
    /// 刷新列表数据
    /// </summary>
    public void RefreshList()
    {
        for (int i = 0; i < mChildren.Count; ++i)
        {
            Transform t = mChildren[i];
            int index = GetIndex(t.gameObject);
            if (index >= from && index <= to)
            {
                UpdateItem(t, index);
            }
        }
        if (wrapContent)
        {
            WrapContent();
        }
    }
    /// <summary>
    /// 摧毁所有成员
    /// </summary>
    /// <param name="includeModel">是否包含模板Item</param>
    public void DestoryAllItem(bool includeModel)
    {
        maxNum = 0;
        for (int i = 0; i < mChildren.Count; ++i)
        {
            Transform t = mChildren[i];
            if (includeModel == false && i == 0)
            {
                t.gameObject.SetActive(false);
            }
            else
            {
                GameObject.Destroy(t.gameObject);
            }
        }
        UpdateScrowView();
        if (wrapContent)
        {
            WrapContent();
        }
    }
    /// <summary>
    /// 移除列表所有成员
    /// </summary>
    public void RemoveAllItem()
    {
        maxNum = 0;
        for (int i = 0; i < mChildren.Count; ++i)
        {
            Transform t = mChildren[i];
            t.gameObject.SetActive(false);
        }
        UpdateScrowView();
        if (wrapContent)
        {
            WrapContent();
        }
    }
    /// <summary>
    /// 移除列表指定成员
    /// </summary>
    /// <param name="item">成员对象</param>
    public void RemoveItem(GameObject item)
    {
        if (item != null)
        {
            int ind = GetIndex(item);
            if (ind >= 0 && ind < maxNum)
            {
                RemoveItem(ind);
            }
        }
    }
    /// <summary>
    /// 移除列表指定成员
    /// </summary>
    /// <param name="index">序号</param>
    public void RemoveItem(int index)
    {
        if (index < 0 && index >= maxNum)
        {
            return;
        }
        maxNum--;
        for (int i = 0; i < mChildren.Count; ++i)
        {
            Transform t = mChildren[i];
            int ind = GetIndex(t.gameObject); 
            if (ind >= index && ind <= to)
            {
                UpdateItem(t, ind);
            }
        }
        UpdateScrowView();
        if (wrapContent)
        {
            WrapContent();
        }
    }
    /// <summary>
    /// 插入列表成员
    /// </summary>
    /// <param name="index">指定序号</param>
    public void InsertItem(int index)
    {
        if (index < 0 && index >= maxNum)
        {
            return;
        }
        maxNum++;
        for (int i = 0; i < mChildren.Count; ++i)
        {
            Transform t = mChildren[i];
            int ind = GetIndex(t.gameObject);
            if (ind >= index && ind <= to)
            {
                UpdateItem(t, ind);
            }
        }
        UpdateScrowView();
        if (wrapContent)
        {
            WrapContent();
        }
    }
    /// <summary>
    /// 追加列表成员
    /// </summary>
    public void AppentItem()
    {
        maxNum++;
        UpdateScrowView();
        if (wrapContent)
        {
            WrapContent();
        }
    }
    /// <summary>
    /// 移动列表
    /// </summary>
    /// <param name="index">指定序号</param>
    public void SpringList(int index)
    {
        if (index < 0 && index >= maxNum)
        {
            return;
        }
        Vector3 panelOffset = GetPanelOffsetByIndex(index);
        Vector3 pos = mPanleLocalPosition;
        if (mHorizontal)
        {
            pos -= new Vector3(panelOffset.x, 0.0f, 0.0f);
            if (autoCenter)
            {
                pos += new Vector3(-itemSize.x / 2.0f + mScroll.panel.finalClipRegion.z / 2.0f, 0.0f, 0.0f);
            }
        } 
        else
        {
            pos -= new Vector3(0.0f, panelOffset.y, 0.0f);
            if (autoCenter)
            {
                pos += new Vector3(0.0f, itemSize.y / 2.0f - mScroll.panel.finalClipRegion.w / 2.0f, 0.0f);
            }
        }
        SpringPanel spring = SpringPanel.Begin(mScroll.panel.gameObject, pos, 10);
        spring.onFinished = SpringListFinishCallback;
    }
    void SpringListFinishCallback()
    {
        SpringPanel spring = mScroll.gameObject.GetComponent<SpringPanel>();
        if (spring != null)
        {
            Destroy(spring);
        }
        ReCenter();
    }
    /// <summary>
    /// 获取指定序号裁剪偏移量
    /// </summary>
    /// <param name="index">指定序号</param>
    /// <returns>裁剪偏移量</returns>
    private Vector3 GetPanelOffsetByIndex(int index)
    {
        Vector3 pos = Vector3.zero;
        if (mHorizontal)
        {
            pos = new Vector3(mPanelClipOffset.x + (float)index * itemSize.x, mScroll.panel.clipOffset.y, 0.0f);
        }
        else
        {
            pos = new Vector3(mScroll.panel.clipOffset.x, mPanelClipOffset.y - (float)index * itemSize.y, 0.0f);
        }
        return pos;
    }
    #endregion
}
