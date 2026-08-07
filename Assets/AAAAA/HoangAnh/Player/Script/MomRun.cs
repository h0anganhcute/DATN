using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class MomRun : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    Animator ani;
    public float thoiGianDelay = 3f;

    private Vector3 viTriBanDau;

    private enum TrangThai { DangDiDenCua, DangCho, DangDiVe, DaVeDenNha }
    private TrangThai trangThaiHienTai = TrangThai.DangDiDenCua;

    void Start()
    {
        ani = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        viTriBanDau = transform.position;

        GameObject door = GameObject.FindGameObjectWithTag("Door");
        if (door != null)
        {
            navMeshAgent.SetDestination(door.transform.position);
            trangThaiHienTai = TrangThai.DangDiDenCua;

            // 1. Bắt đầu chạy đến cửa -> Bật animation chạy
            ani.SetBool("Run", true);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameObject nào có tag là 'Door' trong Scene!");
        }
    }

    void Update()
    {
        if (trangThaiHienTai == TrangThai.DangDiDenCua)
        {
            if (DaDenDichChua())
            {
                StartCoroutine(ChoVaQuayVe());
            }
        }
        else if (trangThaiHienTai == TrangThai.DangDiVe)
        {
            if (DaDenDichChua())
            {
                trangThaiHienTai = TrangThai.DaVeDenNha;

                // 4. Đã về đến nhà và dừng lại -> Tắt animation chạy
                ani.SetBool("Run", false);
                Debug.Log("Đã quay về vị trí ban đầu thành công!");
            }
        }
    }

    private bool DaDenDichChua()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator ChoVaQuayVe()
    {
        trangThaiHienTai = TrangThai.DangCho;

        // 2. Đã đến cửa, đứng chờ -> Tắt animation chạy
        ani.SetBool("Run", false);

        yield return new WaitForSeconds(thoiGianDelay);

        navMeshAgent.SetDestination(viTriBanDau);
        trangThaiHienTai = TrangThai.DangDiVe;

        // 3. Hết thời gian chờ, bắt đầu đi về -> Bật lại animation chạy
        ani.SetBool("Run", true);
    }
   
}