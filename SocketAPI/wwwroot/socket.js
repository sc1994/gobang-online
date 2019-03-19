var connection = new signalR.HubConnectionBuilder()
  .withUrl("http://118.24.27.231:5001/ws?token=123")
  .configureLogging(signalR.LogLevel.Error)
  .build();

connection.start().then(function () {
  console.log("connected");
  window.setInterval(_ => {
    connection.invoke("ping");
  }, 7000);
});

connection.on("Send", msg => {
  console.log(msg);
})

function Send(token, msg) {
  axios.get(`http://118.24.27.231:5001/service/send?token=${token}&msg=${msg}`)
    .then(x => {
      console.log(x);
    })
    .catch(e => {
      console.log(e);
    })
}