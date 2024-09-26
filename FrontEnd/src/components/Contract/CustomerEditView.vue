<template>
    <v-container>
      <v-form>
        <v-row>
          <!-- Họ và tên -->
          <v-col cols="6">
            <v-text-field label="Họ và tên" required></v-text-field>
          </v-col>
  
          <!-- CMND/CCCD -->
          <v-col cols="6">
            <v-text-field label="CMND/CCCD"></v-text-field>
          </v-col>
  
          <!-- Giới tính -->
          <v-col cols="6">
            <v-radio-group v-model="gender" row>
              <v-radio label="Nam" value="male"></v-radio>
              <v-radio label="Nữ" value="female"></v-radio>
            </v-radio-group>
          </v-col>
  
          <!-- Ngày cấp -->
          <v-col cols="6">
            <v-text-field label="Ngày cấp"></v-text-field>
          </v-col>
  
          <!-- Điện thoại 1 -->
          <v-col cols="6">
            <v-text-field label="Điện thoại 1"></v-text-field>
          </v-col>
  
          <!-- Điện thoại 2 -->
          <v-col cols="6">
            <v-text-field label="Điện thoại 2"></v-text-field>
          </v-col>
  
          <!-- Địa chỉ thường trú -->
          <v-col cols="12">
            <v-text-field label="Địa chỉ thường trú"></v-text-field>
          </v-col>
  
          <!-- Ngày sinh -->
          <v-col cols="6">
            <v-text-field label="Ngày sinh (dd/MM/yyyy)"></v-text-field>
          </v-col>
  
          <!-- Nơi sinh -->
          <v-col cols="6">
            <v-select :items="provinces" label="Nơi sinh"></v-select>
          </v-col>
  
          <!-- Tiền phòng -->
          <v-col cols="6">
            <v-text-field label="Tiền phòng" prefix="VND" value="3,000,000"></v-text-field>
          </v-col>
  
          <!-- Đặt cọc -->
          <v-col cols="6">
            <v-text-field label="Đặt cọc" prefix="VND" value="0"></v-text-field>
          </v-col>
  
          <!-- Ngày bắt đầu -->
          <v-col cols="6">
            <v-text-field label="Ngày bắt đầu" value="26/09/2024"></v-text-field>
          </v-col>
  
          <!-- Kỳ thanh toán -->
          <v-col cols="6">
            <v-select :items="paymentTerms" label="Kỳ thanh toán"></v-select>
          </v-col>
  
          <!-- Thanh toán mỗi lần -->
          <v-col cols="6">
            <v-select :items="paymentFrequency" label="Thanh toán mỗi lần"></v-select>
          </v-col>
  
          <!-- Nút lấy khách cũ -->
          <v-col cols="3">
            <v-btn color="primary">Lấy khách cũ</v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-container>
  </template>
  <script>
  export default {
    data() {
      return {
        gender: 'male', // Mặc định là Nam
        provinces: ['Hà Nội', 'TP. Hồ Chí Minh', 'Đà Nẵng'], // Danh sách tỉnh/thành phố
        paymentTerms: ['Kỳ 30', 'Kỳ 60', 'Kỳ 90'], // Kỳ thanh toán
        paymentFrequency: ['1 Tháng', '3 Tháng', '6 Tháng'] // Tần suất thanh toán
      };
    }
  };
  </script>
  