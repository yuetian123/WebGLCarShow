using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Asset.Gufeng.Script
{
    public class MainCamera : MonoBehaviour
    {
        public Transform target;    //���׷��Ŀ��
        public float xSpeed = 200;  //X�᷽���϶��ٶ�
        public float ySpeed = 200;  //Y�᷽���϶��ٶ�
        public float mSpeed = 10;   //�Ŵ���С�ٶ�
        public float yMinLimit = -50; //��Y����С�ƶ���Χ
        public float yMaxLimit = 50; //��Y������ƶ���Χ
        public float distance = 4;  //����ӽǾ���
        public float minDinstance = 2; //����ӽ���С����
        public float maxDinstance = 30; //����ӽ�������
        public float x = 0.0f;
        public float y = 0.0f;
        public float damping = 5.0f;
        public bool needDamping = true;


        // Start is called before the first frame update
        void Start()
        {
            Vector3 angle = transform.eulerAngles;
            x = angle.y;
            y = angle.x - 360f;
        }
        private void Update()
        {

        }
        // Update is called once per frame
        void LateUpdate()
        {
            if (target)
            {
                if (Input.GetMouseButton(0))
                {
                    x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                    y = ClamAngle(y, yMinLimit, yMaxLimit);

                }
                distance -= Input.GetAxis("Mouse ScrollWheel") * mSpeed;
                distance = Mathf.Clamp(distance, minDinstance, maxDinstance);
                Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
                Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * disVector + target.position;

                if (needDamping)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                    transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
                }
                else
                {
                    transform.rotation = rotation;
                    transform.position = position;
                }
            }
        }
        /// <summary>
        /// ����ĳһ���ƶ���Χ
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static float ClamAngle(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}