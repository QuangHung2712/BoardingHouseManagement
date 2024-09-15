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
          <button class="d-flex mt-3">
            <v-icon icon="mdi-key"></v-icon>
            <span class="ml-2">Đổi mật khẩu</span>
          </button>
          <button class="d-flex mt-3">
            <v-icon icon="mdi-exit-to-app"></v-icon>
            <span class="ml-2">Đăng xuất</span>
          </button>
      </div>
    </v-col>
  </v-row>
  <v-row class="m0">
    <v-col cols="2" class="menu">
      <div  >
        <button 
        @click="setActiveMenu(0)"  
        class="menuitem"
        :class="{'active-menu': activeMenu === 0}">
          <span>Trang chủ</span>
        </button>
      </div>
      <div >
        <button 
          @click="setActiveMenu(1)" 
          :class="{'active-menu': activeMenu === 1}" 
          class=" menuitem">
          <v-icon icon="mdi-home" />
          <span>Phòng</span>
        </button>
      </div>
      <div>
        <button 
          @click="setActiveMenu(2)" 
          :class="{'active-menu': activeMenu === 2}" 
          class="menuitem">
          <span>Dịch vụ</span>
        </button>
      </div>
      <div>
        <button 
        @click="setActiveMenu(3)" 
        :class="{'active-menu': activeMenu === 3}" 
        class="menuitem">
          <v-icon icon="mdi-flash"></v-icon>
          <span>Số điện</span>
        </button>
      </div>
      <div>
        <button 
          @click="setActiveMenu(4)" 
          :class="{'active-menu': activeMenu === 4}" 
          class="menuitem">
          <v-icon icon="mdi-water"></v-icon>
          <span>Số nước</span>
        </button>
      </div>
      <div>
        <button 
          @click="setActiveMenu(5)" 
          :class="{'active-menu': activeMenu === 5}" 
          class="menuitem">
          <span>Hợp đồng</span>
        </button>
      </div>
      <div>
        <button 
          @click="setActiveMenu(6)" 
          :class="{'active-menu': activeMenu === 6}" 
          class="menuitem">
          <v-icon icon="mdi-cash"></v-icon>
          <span>Phát sinh</span>
        </button>
      </div>
      <div>
        <button 
          @click="setActiveMenu(7)" 
          :class="{'active-menu': activeMenu === 7}" 
          class="menuitem">
          <v-icon icon="mdi-chart-line"></v-icon>
          <span>Báo cáo</span>
        </button>
      </div>
    </v-col>
    <v-col cols="10">
      <home-page-view v-show="Menu===0"></home-page-view>
      <RoomView v-show="Menu===1"></RoomView>
    </v-col>
  </v-row>
</v-main>
</template>
<script>
import HomePageView from '../../components/HomePageView.vue'
import RoomView from '@/components/RoomView.vue';
  export default {
  components: { HomePageView,RoomView },
    data(){
      return{
        AddressTower:[
          {title: "Số 11 ngõ 91 Cầu Diễn", value: 0},
          {title:"Trâu Quỳ Gia Lâm", value:1}
        ],
        selectTower:0,
        NameLandlord: 'Phạm Quang Hưng',
        activeMenu: null,
        Menu:0,
        diglogProfile: false
      }
    },
    methods: {
      setActiveMenu(menuIndex) {
      this.activeMenu = menuIndex;
      this.Menu = menuIndex
      }
    }
  }
</script>
<style scoped>
.m0{
  margin: 0;
}
.p0{
  padding: 0;
}
.menu button{
  width: 100%;
  text-align: left;
}
.header{
  background-color: #fed9c2;
  height: 12vh;
}
.menu{
  background-color: #fed9c2;
  height: 88vh;
  display: flex;
  flex-direction: column;

}
.menu div{
  flex: 1;
}
.menu .menuitem{
  padding-left:30px;
  display: flex;
  align-items: center;
  font-size: 140%;
}
.menu .menuitem span{
  margin-left: 5px;
  margin-top: 5px;
}
.text-header{
  font-size: 200%;
  margin-left: 20px;
}
.title-address{
  font-size: 130%;
}
.active-menu {
  background-color: #fcb93c;
  color: white;
  border-radius: 20px;
}
.profile{
  width: 100%;
  justify-content: flex-end;
  padding-right: 30px;
}
.diglog_Profile{
  width: 40%;
  border: 1px solid black;
  background-color: aqua;
  margin-left: 50%;
  margin-top: 1%;
  padding: 2%;
  border-radius: 10px;
}
</style>