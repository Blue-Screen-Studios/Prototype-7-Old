const { DataApi } = require('@unity-services/cloud-save-1.2');

module.exports = async ({ params, context, logger }) => {
  const { projectId } = context;
  const { TargetPlayerId } = params;

  // Initialize the cloud save API client
  const cloudSaveAPI = new DataApi(context);

  // Access the save data for the specified player
  const otherPlayerSave = await cloudSaveAPI.getItems(
    projectId,
    TargetPlayerId, // or any other player ID
    "flags" // Cloud Code key
  );

  // Assume that player's likes is 0 if the key doesn't exist
  const targetPlayerFlags = otherPlayerSave.data.results[0];

  return {
    flags: targetPlayerFlags || 0
  }
};