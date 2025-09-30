using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
   private Player _player;
[SerializeField] private Transform attackPoint;
[SerializeField] private float acuracy = 1f,nextAttack =0f;
[SerializeField] private Bullet bulletPrefab;
private Bullet _tempBullet;

   private void Awake()
   {
      _player = GetComponent<Player>();
   }

   private void Update()
   {
      if (!_player.isLocalPlayer) return;
      if (Input.GetKeyUp(KeyCode.Mouse1))
      {
        Shoot();
      }
   }

   public void Shoot()
   {
      if (bulletPrefab && Time.time >nextAttack)
      {
         nextAttack = Time.time + acuracy;
         _tempBullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
         _tempBullet.Shoot();
         if (_player.isLocalPlayer)
         {
            SocketManager.Instance.GetWeaponHandler().CreateShootEvent(GameManager.Instance.localPlayerId,"Shoot");
         }
      }
   }
}
