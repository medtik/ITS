<template>
  <v-container id="content" fluid>
    <v-layout row pa-3>
      <v-flex xs12>
        <span class=title>Account create edit page</span>
        <v-divider class="my-3"></v-divider>
        <v-progress-linear v-if="loading.page" color="primary" indeterminate></v-progress-linear>
        <v-layout column v-else>
          <v-flex>
            <div style="width: 300px">
              <picture-input
                :prefill="photoPrefill"
                width="300"
                height="300"
                accept="image/jpeg,image/png"
                size="50"
                :removable="true"
                buttonClass="v-btn success"
                removeButtonClass="v-btn danger"
                :custom-strings="{
                  change: 'Đổi hình',
                  remove: 'xóa',
                  drag: 'Hình đại diện'
                }"
                @change="onChange"
                @remove="onChange">
              </picture-input>
            </div>
          </v-flex>
          <v-flex style="width: 25rem">
            <v-text-field label="Tên" v-model="nameInput"></v-text-field>
            <v-text-field label="Email" v-model="emailInput"></v-text-field>
            <v-text-field label="Điện thoại" v-model="phoneInput"></v-text-field>
            <v-text-field label="Địa chỉ" v-model="addressInput"></v-text-field>
            <v-text-field label="Ngày sinh" v-model="birthdateInput"></v-text-field>
          </v-flex>

          <v-flex>
            <v-btn color="primary"
                   v-if="mode == 'create'"
                   @click="onCreateClick">
              Tạo mới
            </v-btn>
            <v-btn color="success"
                   v-else
                   :loading="this.loading.updateBtn"
                   @click="onUpdateClick">
              Cập nhật
            </v-btn>
            <v-btn color="secondary"
                   @click="onExitClick">
              Thoát
            </v-btn>
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
  import PictureInput from 'vue-picture-input'

  export default {
    name: "AccountCreateEditView",
    components: {
      PictureInput
    },
    data() {
      return {
        loading: {
          page: true,
          updateBtn: false
        },
        mode: 'create',
        accountId: '',
        account: undefined,
        nameInput: '',
        emailInput: '',
        addressInput: '',
        phoneInput: '',
        birthdateInput: '',
        photoInput: '',
        // set image to this when want to add picture
        photoPrefill: undefined
      }
    },
    created() {
      if (this.$route.name === 'AccountEdit') {
        if (this.$route.query) {
          this.accountId = this.$route.query.id;
          this.mode = 'edit';
          this.$store.dispatch('account/getById', {
            id: this.accountId
          })
            .then(value => {
              this.account = value;
              this.fillInputs()
                .then(() => {
                  this.loading.page = false;
                })

            })
        } else {
          //TODO some error here when no id / wrong id
        }
      } else {
        this.loading.page = false;
      }
    },
    methods: {
      async fillInputs() {
        return new Promise((resolve, reject) => {
          if (this.mode == 'edit' && this.account) {
            this.nameInput = this.account.name;
            this.emailInput = this.account.email;
            this.addressInput = this.account.address;
            this.birthdateInput = this.account.birthdate;
            this.phoneInput = this.account.phone;
            if (this.account.photo) {
              this.photoInput = this.account.photo;
              this.base64toFile(this.account.photo)
                .then(value => {
                  this.photoPrefill = value;
                  resolve();
                })
            } else {
              resolve();
            }
          } else {
            reject();
          }
        })
      },
      onChange(image) {
        this.photoInput = image;
      },
      onCreateClick() {

      },
      onUpdateClick() {
        this.loading.updateBtn = true;
        this.$store.dispatch('account/update', {
          id: this.accountId,
          name: this.nameInput,
          email: this.emailInput,
          phone: this.phoneInput,
          address: this.addressInput,
          birthdate: this.birthdateInput,
          photo: this.photoInput
        })
          .then(value => {
            this.loading.updateBtn = false
          })
      },
      onExitClick() {
        this.$router.back();
      },
      base64toFile(url, filename = 'randomname', mimeType) {

        mimeType = mimeType || (url.match(/^data:([^;]+);/) || '')[1];
        return (fetch(url)
            .then(function (res) {
              return res.arrayBuffer();
            })
            .then(function (buf) {
              return new File([buf], filename, {type: mimeType});
            })
        );
      }
    }
  }
</script>

<style scoped>

</style>
