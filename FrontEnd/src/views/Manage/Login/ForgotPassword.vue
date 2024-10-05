<template>
    <v-sheet class="bg-deep-purple pa-6" rounded>
      <v-card class="mx-auto px-6 py-8">
        <h1 class="text-center mb-8">Quên mật khẩu</h1>
        <v-form
          v-model="form"
          @submit.prevent="onSubmit"
        >
          <v-text-field
            v-model="email"
            :readonly="loading"
            :rules="[required]"
            class="mb-2"
            label="Email"
            clearable
          ></v-text-field>
          
          <v-row class="m0 mb-2">
            <v-col cols="4" class="p0 text-center mt-2">
              <V-btn @click="SendVetifiCode()">Gửi mã</V-btn>
            </v-col>
            <v-col cols="8" class="p0">
              <v-text-field
                v-model="verifiCode"
                type="number"
                :readonly="loading"
                :rules="[checkCode]"
                :maxlength="6"
                label="Mã xác nhận"
                clearable
              ></v-text-field>
            </v-col>
          </v-row>
          <div v-show="isCodeValid">
            <v-text-field
              v-model="password"
              :readonly="loading"
              :rules="[required]"
              label="Password"
              placeholder="Enter your password"
              :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="visible ? 'text' : 'password'"
              @click:append-inner="visible = !visible"
              clearable
            ></v-text-field>
            <v-text-field
              v-model="ConfirmPassword"
              :readonly="loading"
              :rules="[required]"
              label="Nhập lại mật khẩu"
              placeholder="Enter your password"
              :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="visible ? 'text' : 'password'"
              @click:append-inner="visible = !visible"
              clearable
            ></v-text-field>
          </div>
          <br>
  
          <v-btn
            :disabled="!form"
            :loading="loading"
            color="success"
            size="large"
            type="submit"
            variant="elevated"
            block
          >
            Đổi mật khẩu
          </v-btn>
          <router-link to="/login">Đăng nhập</router-link>
        </v-form>
      </v-card>
    </v-sheet>
  </template>
  
  <script>
    export default {
      data: () => ({
        form: false,
        email: "",
        password: "",
        loading: false,
        visible: false,
        ConfirmPassword: '',
        verifiCode: null,
        code:0,
        isCodeValid : false
      }),
      computed:{
        checkCode(){
          return v => {
            if (v == null || v === "") {
              return "Mã xác nhận không được để trống";
            } else if (v !== this.code.toString()) {
              this.isCodeValid = false; // Không hợp lệ
              return "Mã xác nhận không đúng";
            }
            this.isCodeValid = true; // Hợp lệ
            return true;
          }
        }
      },
      methods: {
        onSubmit () {
          if (!this.form) return
  
          this.loading = true
  
          setTimeout(() => (this.loading = false), 2000)
        },
        required (v) {
          return !!v || 'Không được để trống'
        },
        SendVetifiCode(){
          this.code = Math.floor(100000 + Math.random() * 900000);
          alert(this.code)
        }
      },
    }
  </script>

  <style scoped>
    .v-card{
      width: 50%;
    }
  </style>
  