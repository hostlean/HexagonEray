
using UnityEngine;
using Random = UnityEngine.Random;

public class Hexagon : MonoBehaviour
{
   public Vector2 Center { get; set; }
   public float OuterRadius { get; set; }

   public float InnerRadius
   {
      get
      {
         {
            var inner = (Mathf.Sqrt(3) * OuterRadius) / 2;
            return inner;
         }
      }
   }

   [SerializeField] private SpriteRenderer spriteRenderer;

   [SerializeField] private PolygonCollider2D polygonCollider2D;

   [SerializeField] private GameObject outline;

   [SerializeField] private float moveSpeed;

   private Vector2 pos;

   public Vector2 TargetPos { get; set; }

   private Transform _holderParent;

   public bool AtTarget { get; set; }

   private bool _canScore;

   public enum HexagonType
   {
      Red = 0,
      Orange = 1,
      Yellow = 2,
      Blue = 3,
      Purple = 4,
      Green = 5,
   }

   public HexagonType hType;

   private void OnEnable()
   {
      gameObject.layer = Constants.Layers.Default;
      int randomColorIndex = Random.Range(0, HexagonManager.Instance.Colors.Length);
      spriteRenderer.color = HexagonManager.Instance.Colors[randomColorIndex];
      polygonCollider2D.enabled = false;
      hType = (HexagonType) randomColorIndex;
      _holderParent = transform.parent;
   }

   private void Update()
   {
      MoveToTarget();
   }

   private void MoveToTarget()
   {
      if (AtTarget == false)
      {
         polygonCollider2D.enabled = false;
         transform.position = Vector2.MoveTowards(transform.position,
            TargetPos, moveSpeed * Time.deltaTime);
         
         if (transform.position == (Vector3) TargetPos)
         {
            polygonCollider2D.enabled = true;
            AtTarget = true;
            _canScore = true;
            gameObject.layer = Constants.Layers.hexagon;
         }
      }
   }

   public void ActivateOutline()
   {
      outline.SetActive(true);
      spriteRenderer.sortingLayerName = Constants.SortingLayers.chosenHexagon;
   }

   public void DeactivateOutline()
   {
      outline.SetActive(false);
      spriteRenderer.sortingLayerName = Constants.SortingLayers.hexagon;
   }

   public void ApplyParent(GameObject go)
   {
      transform.parent = go.transform;
   }

   public void ReturnHolderParent()
   {
      transform.parent = _holderParent;
   }

   private void OnDisable()
   {
      polygonCollider2D.enabled = false;
      gameObject.layer = Constants.Layers.Default;
      
      if (_canScore)
      {
         GameManager.Instance.Score += 5;
         UIManager.Instance.UpdateText(5);
         _canScore = false;
      }

      AtTarget = false;
      gameObject.layer = Constants.Layers.Default;

   }
}
