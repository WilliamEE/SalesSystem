import '@babel/polyfill'
import Vue from 'vue'
import './plugins/vuetify'
import App from './App.vue'
import router from './router'
import store from './store'
import axios from 'axios'
import firebase from "firebase/app";
import 'firebase/auth';

Vue.config.productionTip = false
const configOptions = {
  apiKey: "AIzaSyB4vKgM1IjojHLhBPTFEN0nTset-gSdpQ0",
  authDomain: "dsi215.firebaseapp.com",
  databaseURL: "https://dsi215.firebaseio.com",
  projectId: "dsi215",
  storageBucket: "dsi215.appspot.com",
  messagingSenderId: "1058854973652",
  appId: "1:1058854973652:web:e3faa8c77b23ab69e44b41",
  measurementId: "G-7BGVMZ4PER"
};
firebase.initializeApp(configOptions);

axios.defaults.baseURL='http://ec2-34-239-124-79.compute-1.amazonaws.com/'
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
