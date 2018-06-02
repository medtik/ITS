import 'package:flutter/material.dart';

class QuestionPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      appBar: new AppBar(
          backgroundColor: Colors.orange,
          title: new Text("Intelligent Tourist")),
      body: new Container(
          decoration: new BoxDecoration(
            image: new DecorationImage(
              image: new AssetImage("assets/question_background.png"),
              fit: BoxFit.cover,
            ),
          ),
          child: new Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: <Widget>[
              new Container(
                child: new Text(
                  "Chào mừng bạn đến với hệ thống hướng dẫn du lịch thông minh",
                  style: new TextStyle(color: Colors.white, fontSize: 25.0),
                  textAlign: TextAlign.center,
                ),
                padding: EdgeInsets.symmetric(vertical: 0.0, horizontal: 45.0),
                margin: EdgeInsets.symmetric(vertical: 80.0, horizontal: 0.0),
              ),
              new Container(
                margin: EdgeInsets.only(bottom: 20.0),
                child: new SizedBox(
                  width: 280.0,
                  height: 130.0,
                  child: new MaterialButton(
                    child: new Text("Tìm kiếm thông minh",
                        style: new TextStyle(
                            fontWeight: FontWeight.bold, fontSize: 16.0)),
                    color: Colors.deepOrange,
                    textColor: Colors.white,
                    onPressed: () {},
                  ),
                ),
              ),
              new Container(
                child: new SizedBox(
                  width: 280.0,
                  height: 80.0,
                  child: new MaterialButton(
                    child: new Text("Tìm kiếm thông thường",
                        style: new TextStyle(
                            fontWeight: FontWeight.bold, fontSize: 16.0)),
                    color: Colors.blue,
                    textColor: Colors.white,
                    onPressed: () {},
                  ),
                ),
              ),
            ],
          )),
    );
  }
}
