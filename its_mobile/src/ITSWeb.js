import React from "react" ;
import {View, WebView, StyleSheet, Dimensions, BackHandler} from "react-native" ;
import firestore from './firestore';
import notification from './SetupNotification';

const ref = {
    webview: "WEBVIEW_REF"
};
export default class ITSWeb extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            // uri: 'http://its8.gear.host/',
            uri: 'http://192.168.150.80:80',
            canGoBack: false
        };

        this.onLayout = this.onLayout.bind(this);
        this.onHardwareBack = this.onHardwareBack.bind(this);
        this.onNavigationStateChange = this.onNavigationStateChange.bind(this);
        this.onMessage = this.onMessage.bind(this);
    }

    componentDidMount() {
        this.onLayout();
        BackHandler.addEventListener('hardwareBackPress', this.onHardwareBack);
    }

    componentWillUnmount() {
        BackHandler.removeEventListener('hardwareBackPress', this.onHardwareBack);
    }

    onLayout() {
        const dimension = Dimensions.get('window');
        this.setState({
            width: dimension.width,
            height: dimension.height
        });
    }

    onHardwareBack() {
        if (this.state.canGoBack) {
            this.refs[ref.webview].goBack();
        }
        return this.state.canGoBack;
    }

    onNavigationStateChange(ex) {
        this.setState({
            canGoBack: ex.canGoBack
        });
    }

    onMessage(data) {
        let obj = JSON.parse(data);
        if (obj.type === 'uid') {
            notification.registerForPushNotificationsAsync()
                .then(value => {
                    firestore
                        .collection('user')
                        .doc(obj.uid)
                        .update({
                            token: value
                        })
                })

        }
    }

    render() {
        const style = StyleSheet.create({
            container: {
                backgroundColor: "#007AFF",
                paddingTop: 23
            },
            webView: {
                width: this.state.width,
                height: this.state.height,
            }
        });

        return (
            <View style={style.container}
                  onLayout={this.onLayout}>
                <WebView
                    ref={ref.webview}
                    onNavigationStateChange={this.onNavigationStateChange}
                    onMessage={(event) => this.onMessage(event.nativeEvent.data)}
                    source={{uri: this.state.uri}}
                    style={style.webView}
                    userAgent="its8_demo"
                />
            </View>
        );
    }
}