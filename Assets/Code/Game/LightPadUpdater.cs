using UnityEngine;

namespace Assembly.IBX.Main
{
    public class LightPadUpdater : MonoBehaviour
    {
        [SerializeField] private Material lightPadShaderMat;

        [SerializeField] private Transform player1;
        [SerializeField] private Transform player2;
        [SerializeField] private Transform player3;

        private void Update()
        {
            Vector2 shaderMapSize = lightPadShaderMat.GetVector("_Map_Size");

            Vector2 player1ShaderPos = new Vector2(player1.position.x - shaderMapSize.x / 2, player1.position.z - shaderMapSize.y / 2);
            Vector2 player2ShaderPos = new Vector2(player2.position.x - shaderMapSize.x / 2, player2.position.z - shaderMapSize.y / 2);
            Vector2 player3ShaderPos = new Vector2(player3.position.x - shaderMapSize.x / 2, player3.position.z - shaderMapSize.y / 2);

            lightPadShaderMat.SetVector("_Player_1_Position", player1ShaderPos);
            lightPadShaderMat.SetVector("_Player_2_Position", player2ShaderPos);
            lightPadShaderMat.SetVector("_Player_3_Position", player3ShaderPos);
        }
    }
}
