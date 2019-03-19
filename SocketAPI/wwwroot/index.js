var vm = new Vue({
  el: "#app",
  data: {
    self: "",
    personnel: [],
    control: ""
  },
  watch: {
    "personnel": function (val) {
      var i = val.indexOf(this.self);
      if (i == 0) {
        this.control = "b"; //执黑
      } else if (i == 1) {
        this.control = "w"; // 执白
      } else {
        this.control = "k"; // 观战
      }
    }
  },
  mounted: function () {
    var room = UrlKey("room");
    if (!room || room == "") {
      room = RandomToken();
      location.href += `?room=${room}`;

    } else {

    }
    init(room);
  }
});