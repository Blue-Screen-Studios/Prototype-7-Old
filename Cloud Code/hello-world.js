module.exports = async ({ params, logger }) => {
  const name = params.name;
  const message = `Hello, ${name}. Welcome to Cloud Code!`

  logger.debug(message);
  return {
    welcomeMessage: message
  };
};