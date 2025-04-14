using ToyBox.Player;
using UnityEngine;

public class Fllamethrower : MonoBehaviour
{
    //⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣟⣿
    //⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠈⠉⢸⣿
    //⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠟⢙⣷⣶⢶⣿⣿
    //⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⡙⢷⣾⣿⠟⢁⣼⣿⣿
    //⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠿⠿⠿⠛⣧⣼⣷⡾⠋⣁⣴⣿⣿⣿⣿
    //⣿⣿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⢿⣏⣠⠖⢀⣠⣾⡿⠟⢁⣤⣾⣿⣿⣿⣿⣿⣿
    //⣿⣿⣿⣿⣿⣿⡿⠛⡷⠀⠈⢿⣾⠋⠀⢴⣿⠟⠋⣠⣶⣿⣿⣿⣿⣿⣿⣿⣿⣿
    //⣿⣿⣿⣿⡟⠻⠆⠀⠀⠀⣠⣾⣿⣿⡷⠟⠻⣶⣿⣿⣿⣿⠿⣿⣿⣿⢿⣿⣿⣿
    //⣿⣿⣿⣿⣧⡀⠀⢀⣴⣾⠉⢸⣿⣋⣠⣤⣴⡿⠟⠛⣿⠁⠀⠈⣿⠁⠀⢹⣿⣿
    //⣿⣿⣿⣿⣿⣿⠞⠋⠁⣿⣄⣸⣿⣿⣿⣿⠏⢠⣶⣿⣿⣀⣀⣀⣿⣀⣀⣸⣿⣿
    //⣿⣿⣿⣿⣿⠇⢀⣤⣾⣿⣿⢹⣿⣿⣿⡿⠀⣿⣿⣿⣉⣉⣉⣉⣉⣉⣉⣉⣹⣿
    //⣿⣿⡿⠏⠁⠀⢸⣿⣿⣿⣿⠘⣿⣿⣿⡇⢰⣿⣿⣿⣿⠉⣿⠉⠉⠉⡏⢹⣿⣿
    //⣿⡋⠀⠀⠀⢠⣿⣿⣿⣿⣿⡆⢻⣿⣿⡇⢸⣿⣿⣿⣿⠀⣿⣀⣀⣠⡇⢸⣿⣿
    //⣿⣧⠀⠀⣰⣿⣿⣿⣿⣿⣿⣷⣄⠙⠋⣠⣾⣿⣿⣿⣿⠀⠈⠉⣿⠉⠁⢸⣿⣿
    //⣿⣿⣷⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣤⣤⣤⣿⣤⣤⣼⣿⣿
    
    [SerializeField] float _range;
    [SerializeField] GameObject _projectile;
    
    void Update()
    {
        RaycastHit2D[] hitList = Physics2D.RaycastAll(transform.position, transform.up, _range);

        foreach (RaycastHit2D hit in hitList)
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerMovement player))
            {
                Debug.Log("Hit Player");
                Shoot();
            }
        }
        
    }

    void Shoot()
    {
        GameObject bolt = Instantiate(_projectile, transform.position, Quaternion.identity);
        bolt.GetComponent<Rigidbody2D>().AddForce(transform.up * 40f, ForceMode2D.Impulse);
        
        Destroy(bolt, 0.2f);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * _range);
    }
}































































//⠀⠀⠀⠀⠀⠀⠀⠀⢀⣤⣶⣶⣤⡀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⠀⣾⣿⣿⣿⣿⣿⡄⠀
//⠀⠀⠀⠀⠀⠀⠀⠀⠹⣿⣿⣿⣿⡿⠁⠀
//⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠉⠉⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⣰⣿⣿⣿⣦⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⣰⣿⣿⣿⣿⣿⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⢰⣿⣿⣿⣿⣿⣿⡀⠀⠀⠀
//⠀⠀⠀⠀⢠⣿⣿⣿⣿⡿⣿⣿⣧⣀⠀⠀
//⠀⠀⠀⠀⢺⣿⣿⣿⣿⣧⣬⣻⢿⣿⣿⡦
//⠀⠀⠀⠀⠀⠙⠻⠿⢿⣿⣿⣿⣿⡏⠛⠁
//⠀⠀⠀⣀⠀⠀⠀⠀⠀⠀⣽⣿⡿⠁⠀⠀
//⠀⢀⡠⣿⣷⣤⡀⠀⠀⢸⣿⣿⠃⠀⠀⠀
//⠰⠿⠿⠿⠿⠿⠇⠀⠠⠿⠿⠏⠀⠀⠀⠀
