<template>
<v-main>
  <v-row class="m0 header d-flex">
    <v-col cols="4" >
      <button class="text-left text-header">QUẢN LÝ NHÀ TRỌ</button>
    </v-col>
    <v-col cols="4">
      <v-row v-show="!AddressTower" class="m0 d-flex align-center">
        <v-col cols="3" class="p0 mb-5 title-address">
          Địa chỉ: 
        </v-col>
        <v-col cols="9" class="p0">
          <v-select 
          v-model="selectTower"
          :items="AddressTower"
          item-title="title"
          item-value="value"></v-select>
        </v-col>
      </v-row>
    </v-col>
    <v-col cols="4" class="p0	">
      <button @click="diglogProfile = !diglogProfile" class=" profile d-flex mt-8 mr-5">
        <p>{{ NameLandlord }}</p>
        <v-icon icon="mdi-menu-down"></v-icon>
      </button>
      <div v-show="diglogProfile" class="diglog_Profile">
          <button class="d-flex">
            <v-icon icon="mdi-account"></v-icon>
            <span class="ml-2">Thông tin cá nhân</span>
          </button>
          <button class="d-flex mt-3" @click="btnChangePassword()">
            <v-icon icon="mdi-key"></v-icon>
            <span class="ml-2">Đổi mật khẩu</span>
          </button>
          <router-link class="d-flex mt-3" to="/login">
            <v-icon icon="mdi-exit-to-app"></v-icon>
            <span class="ml-2">Đăng xuất</span>
          </router-link>
      </div>
    </v-col>
  </v-row>
  <router-link to="/"></router-link>
  <router-view></router-view>
  <v-dialog v-model="dialogEdit" class="dialog">
    <v-card>
        <v-card-title>{{titleDialog}}</v-card-title>
        <v-form v-model="form" class="content">
          <v-row class="p0 m0">
            <v-col offset="2" cols="2" class="mt-5">Mật khẩu cũ</v-col>
            <v-col cols="6">              
              <v-text-field
              v-model="password"
              label="Mật khẩu cũ"
              placeholder="Nhập mật khẩu cũ"
              :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="visible ? 'text' : 'password'"
              @click:append-inner="visible = !visible"
              clearable
              ></v-text-field>
            </v-col>
            <v-col cols="2"></v-col>
          </v-row>
          <v-row class="p0 m0">
            <v-col offset="2" cols="2" class="mt-5">Mật mới</v-col>
            <v-col cols="6">
              <v-text-field
              v-model="password"
              label="Mật khẩu mới"
              placeholder="Nhập mật khẩu mới"
              :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="visible ? 'text' : 'password'"
              @click:append-inner="visible = !visible"
              clearable
              ></v-text-field>
            </v-col>
            <v-col cols="2"></v-col>
          </v-row>
          <v-row class="p0 m0">
            <v-col offset="2" cols="2" class="mt-5">Nhập lại mật khẩu</v-col>
            <v-col cols="6">              
              <v-text-field
              v-model="password"
              label="Mật khẩu mới"
              placeholder="Nhập lại mật khẩu mới"
              :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="visible ? 'text' : 'password'"
              @click:append-inner="visible = !visible"
              clearable
              ></v-text-field>
            </v-col>
            <v-col cols="2"></v-col>
          </v-row>
        </v-form>
        <v-card-actions class="justify-space-between">
            <v-btn @click="dialogEdit=false">Hủy</v-btn>
            <v-btn :disabled="!form" @click="btnSave">Lưu</v-btn>
        </v-card-actions>
    </v-card>
  </v-dialog>
</v-main>
</template>
<script>
  export default {
    data(){
      return{
        AddressTower:[
          {title: "Số 11 ngõ 91 Cầu Diễn", value: 0},
          {title:"Trâu Quỳ Gia Lâm", value:1}
        ],
        selectTower:0,
        NameLandlord: 'Phạm Quang Hưng',
        diglogProfile: false,
        dialogEdit: false,
        titleDialog: '',
        visible: false,
        form: false,
      }
    },
    methods: {
      setActiveMenu(menuIndex) {
      this.activeMenu = menuIndex;
      this.Menu = menuIndex
      },
      btnChangePassword(){
        this.dialogEdit = true,
        this.titleDialog = 'Đổi mật khẩu'
      },
      btnSave(){
        if(!this.form){
          return;
        }
        alert('form đã được submit')
      }
    },

  }
</script>
<style scoped>
.m0{
  margin: 0;
}
.p0{
  padding: 0 !important;
}
.text-header{
  font-size: 200%;
  margin-left: 20px;
}
.title-address{
  font-size: 130%;
}
.header{
  background-color: #fed9c2;
  height: 12vh;
}
.profile{
  width: 100%;
  justify-content: flex-end;
  padding-right: 30px;
}
.diglog_Profile{
  border: 1px solid black;
  background-color: aqua;
  padding: 1%;
  border-radius: 10px;
  position: absolute;
  top: 9%; /* Điều chỉnh khoảng cách từ đỉnh */
  right: 2%; /* Điều chỉnh khoảng cách từ cạnh phải */
  z-index: 2000; /* Đảm bảo nằm trên tất cả các thành phần khác */
}
.diglog_Profile .mt-3{
  text-decoration: none !important;
  color: black;
}
.dialog{
    width: 80%;
    height: 80vh;
}
.dialog .v-btn{
    background-color: blue;
    color: whitesmoke;
}
.dialog .v-text-field{
  width: 80%;
}
.content{
  background-color: #D9D9D9;
  margin-left: 5%;
  margin-right: 5%;
  border-radius: 25px;
}
</style>