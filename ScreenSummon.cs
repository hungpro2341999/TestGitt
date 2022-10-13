using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using DG.Tweening;
using Sfs2X.Entities.Data;

public class ScreenSummon : Panel
{
    public Button btnX;

    public Text txtStone1;
    public Text txtStone2;
    public Text txtDiamondRequire1;
    public Text txtDiamondRequire2;
    public Button btnRoll1;
    public Button btnRoll5;
    public SkeletonGraphic fxBGSummon;
    private System.Action actionNextSummon;
    public UISummon uISummon;
    
    // Start is called before the first frame update
           
    void Start()
    {
        btnX.onClick.AddListener(() =>
        {
            ScreenUI.Instance.Hide<ScreenSummon>();
            CtrlUI.Instance.OpenScreenCampaign();
        });
        fxBGSummon.Initialize(true);
        int a = 0;
        int b = -1;
        fxBGSummon.AnimationState.Event += (TrackEntry trackEntry, Spine.Event e) =>
        {
            if(trackEntry.Animation.Name.Equals("star1"))
            {
                if (e.Data.Name.Equals("even_star"))
                {
                    Debug.Log("Star1 ");
                    fxBGSummon.AnimationState.AddAnimation(a++, "star2", false, 0f);
                }
            }
            else if (trackEntry.Animation.Name.Equals("star2"))
            {
                if (e.Data.Name.Equals("even_star"))
                {
                    Debug.Log("Star2 ");
                    fxBGSummon.AnimationState.ClearTrack(a-1);
                    fxBGSummon.AnimationState.AddAnimation(a++, "star1", false, 0f);
                }
            }
        };
        fxBGSummon.AnimationState.AddAnimation(a, "star1",true, 0f);

    }

    public void Load(UISummon uiSummon)
    {
        this.uISummon = uiSummon;
        if (uiSummon.stone1 >= CtrlData.Instance.userInfor.souldStone)
        {
            txtStone1.color = CtrlData.Instance.colorEnoughStamina;
        }
        else
        {
            txtStone1.color = CtrlData.Instance.colorNotEnoughStamina;
        }
        if (uiSummon.stone2 >= CtrlData.Instance.userInfor.souldStone)
        {
            txtStone2.color = CtrlData.Instance.colorEnoughStamina;
        }
        else
        {
            txtStone2.color = CtrlData.Instance.colorNotEnoughStamina;
        }
        txtStone1.text = uiSummon.stone1.ToString()+"/"+CtrlData.Instance.userInfor.isouldStone;
        txtStone2.text = uiSummon.stone2.ToString()+"/"+CtrlData.Instance.userInfor.isouldStone;
        txtStone1.color = (uiSummon.stone1 <= CtrlData.Instance.userInfor.isouldStone) ? CtrlData.Instance.colorEnoughStamina : CtrlData.Instance.colorNotEnoughStamina;
        txtStone2.color = (uiSummon.stone2 <= CtrlData.Instance.userInfor.isouldStone) ? CtrlData.Instance.colorEnoughStamina : CtrlData.Instance.colorNotEnoughStamina;
        txtDiamondRequire1.text = uiSummon.diamond1.ToString();
        txtDiamondRequire2.text = uiSummon.diamond2.ToString();
        btnRoll1.onClick.RemoveAllListeners();
        btnRoll1.onClick.AddListener(() =>
        {
            SFSObject sfsObject = new SFSObject();
            sfsObject.PutByte("summonType", (byte)1);
            CtrlUI.Instance.OpenLoadingProcessSmartFox(SmartFoxConnection.SFSAction.TRANDITIONAL_SUMMON, sfsObject, TypeBlock.Waiting, ProcessSummon);
            OfferManager.Instance.ShowRoll(1);
        });
        btnRoll5.onClick.RemoveAllListeners();
        btnRoll5.onClick.AddListener(() =>
        {
            SFSObject sfsObject = new SFSObject();
            sfsObject.PutByte("summonType",(byte)5);
            CtrlUI.Instance.OpenLoadingProcessSmartFox(SmartFoxConnection.SFSAction.TRANDITIONAL_SUMMON, sfsObject, TypeBlock.Waiting, ProcessSummon);
            OfferManager.Instance.ShowRoll(5);
        });
        SetUpTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ProcessSummon(int action,int errorCode,ISFSObject sFSObject)
    {
       switch(action)
        {
            case SmartFoxConnection.SFSAction.TRANDITIONAL_SUMMON:
                if (errorCode != 0)
                {
                    DialogToast.Ins.ShowToast(Config.NOT_ENOUGH_MONEY);
                    return;
                }
                UISummonResult uISummonResult = new UISummonResult();
                uISummonResult.Load(sFSObject);
                PopUpSummonResult popUpSummonResult = null;
                PopupUI.Instance.Show<PopUpSummonResult>(out popUpSummonResult, null, Menu<PopupUI>.ShowType.DissmissCurrent);
                popUpSummonResult.Load(uISummonResult);
                popUpSummonResult.btnSummonNext.onClick.AddListener(() =>
                {

                });
                Debug.Log("summon");
                break;
        }
    }
}
