import { createRouter, createWebHistory } from "vue-router";
import { useProductRoute } from "../stores/useProductRoute";
import { storeToRefs } from "pinia";
import User from "../views/user/User.vue";
import Favorites from "../views/user/Favorites.vue";
import Coupon from "../views/user/Coupon.vue";
import Login from "../views/user/Login.vue";
import ResetPassword from "../views/user/ResetPassword.vue";
import ActivationAcc from "../views/user/ActivationAcc.vue";
import ActivityInfo from "../views/activity/ActivityInfo.vue";
import ActivitySignUp from "../views/activity/ActivitySignUp.vue";
import ActivityIndex from "../views/activity/ActivityIndex.vue";
import ReservationIndex from "../views/reservation/ReservationIndex.vue";
import SpeakerInfo from "../views/reservation/SpeakerInfo.vue";
import PaymentSuccess from "../views/activity/PaymentSuccess.vue";
import Community from "../views/activity/Community.vue";
const webTitle = "FLEX - ";

// 路由設定
const routes = [
  {
    //http://loaclhost/
    path: "/",
    component: () => import("@/views/home/Home.vue"),
    meta: { title: `${webTitle}首頁` },
  },
  {
    //http://loaclhost/cart
    path: "/cart",
    component: () => import("@/views/home/Cart.vue"),
    meta: { title: `${webTitle}購物車`, require: true },
  },
  {
    //http://loaclhost/buy
    path: "/buy",
    component: () => import("@/views/home/Buy.vue"),
    meta: { title: `${webTitle}結帳`, require: true },
  },
  {
    //http://loaclhost/onSale
    path: "/onSale",
    component: () => import("@/views/home/Sale.vue"),
    meta: { title: `${webTitle}特惠專區` },
  },
  {
    //http://loaclhost/flexClub
    path: "/flexClub",
    component: () => import("@/views/activity/FlexClub.vue"),
    meta: { title: `${webTitle}運動俱樂部` },
  },
  {
    //http://loaclhost/User
    path: "/user",
    component: User,
    meta: { title: `${webTitle}個人資料`, require: true },
  },
  {
    //http://loaclhost/Favorites
    path: "/favorites",
    component: Favorites,
    meta: { title: `${webTitle}收藏清單`, require: true },
  },
  {
    //http://loaclhost/Coupon
    path: "/coupon",
    component: Coupon,
    meta: { title: `${webTitle}優惠券`, require: true },
  },
  {
    //http://loaclhost/Login
    path: "/login",
    component: Login,
  },
  {
    //http://loaclhost/ResetPassword
    path: "/ResetPassword",
    component: ResetPassword,
    meta: { title: `${webTitle}重設密碼` },
  },
  {
    //http://loaclhost/ActivationAcc
    path: "/ActivationAcc",
    component: ActivationAcc,
    meta: { title: `${webTitle}啟用帳戶` },
  },
  {
    //http://loaclhost/orders
    path: "/orders",
    component: () => import("../views/orders/orderindex.vue"),
    meta: { title: `${webTitle}訂單` },
  },
  {
    path: "/activityIndex",
    component: ActivityIndex,
    meta: { title: `${webTitle}活動首頁` },
  },
  {
    //http://loaclhost/activityInfo
    path: "/activityInfo/:id",
    component: ActivityInfo,
    meta: { title: `${webTitle}活動` },
  },
  {
    //http://loaclhost/activitySignUp
    path: "/activitySignUp/:id",
    component: ActivitySignUp,
    name: "activitySignUp",
    meta: { title: `${webTitle}活動報名`, require: true },
  },
  {
    //http://loaclhost/Community
    path: "/community/:category",
    component: Community,
    meta: { title: `${webTitle}活動心得` },
  },
  {
    // http://loaclhost/paymentSuccess
    path: "/paymentSuccess/:TradeAmt/:TradeNo/:ActivityName",
    component: PaymentSuccess,
    meta: { title: `${webTitle}訂單資訊` },
  },
  {
    path: "/reservationIndex",
    component: ReservationIndex,
    meta: { title: `${webTitle}預約諮詢首頁` },
  },
  {
    path: "/speakerInfo/:id",
    component: SpeakerInfo,
    meta: { title: `${webTitle}講師資訊` },
  },
  {
    //http://loaclhost/orders
    path: "/orders",
    component: () => import("../views/orders/orderindex.vue"),
    meta: { title: `${webTitle}訂單` },
  },
  {
    //http://loaclhost/Login
    path: "/login",
    component: Login,
  },
  {
    //http://localhost/Search
    path: "/search/:keyWord",
    component: () => import("../views/product/ProductKeywordSearch.vue"),
    meta: { title: `${webTitle}商品搜尋` },
  },
  {
    //http://loaclhost/Men
    path: "/men",
    component: () => import("../views/product/ProductMenLayout.vue"),
    children: [
      {
        path: "",
        component: () => import("../views/product/ProductList.vue"),
        meta: { title: `${webTitle}男裝` },
      },
      {
        path: ":categoryName",
        component: () => import("../views/product/ProductList.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          document.title = `${webTitle}男裝/${to.params.categoryName}`;
          next();
        },
      },
      {
        path: ":categoryName/:subCategoryName",
        component: () => import("../views/product/ProductList.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          document.title = `${webTitle}男裝/${to.params.categoryName}/${to.params.subCategoryName}`;
          next();
        },
      },
      {
        // 當 /ProductMenLayout/:id/posts 匹配成功
        // Detial.vue 將被渲染到 ProductMenLayout 的 <router-view> 内部，替換card.vue
        path: "detail/:productId",
        component: () => import("../views/product/ProductDetail.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          // const productStore = useProductRoute();
          // const productName = productStore.productName;
          document.title = `${webTitle}男裝`; //productName ${to.params.productId}
          next();
        },
      },
    ],
  },
  {
    //http://loaclhost/Women
    path: "/women",
    component: () => import("../views/product/ProductWomenLayout.vue"),
    children: [
      {
        path: "",
        component: () => import("../views/product/ProductList.vue"),
        meta: { title: `${webTitle}女裝` },
      },
      {
        path: ":categoryName",
        component: () => import("../views/product/ProductList.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          document.title = `${webTitle}女裝/${to.params.categoryName}`;
          next();
        },
      },
      {
        path: ":categoryName/:subCategoryName",
        component: () => import("../views/product/ProductList.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          document.title = `${webTitle}女裝/${to.params.categoryName}/${to.params.subCategoryName}`;
          next();
        },
      },
      {
        // 當 /ProductMenLayout/:id/posts 匹配成功
        // Detial.vue 將被渲染到 ProductMenLayout 的 <router-view> 内部，替換card.vue
        path: "detail/:productId",
        component: () => import("../views/product/ProductDetail.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          document.title = `${webTitle}女裝`; //${to.params.productId}
          next();
        },
      },
    ],
  },
  {
    //http://loaclhost/Kid
    path: "/kid",
    component: () => import("../views/product/ProductKidLayout.vue"),
    children: [
      {
        path: "",
        component: () => import("../views/product/ProductList.vue"),
        meta: { title: `${webTitle}童裝` },
      },
      {
        path: ":categoryName",
        component: () => import("../views/product/ProductList.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          document.title = `${webTitle}童裝/${to.params.categoryName}`;
          next();
        },
      },
      {
        path: ":categoryName/:subCategoryName",
        component: () => import("../views/product/ProductList.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          document.title = `${webTitle}童裝/${to.params.categoryName}/${to.params.subCategoryName}`;
          next();
        },
      },
      {
        // 當 /ProductMenLayout/:id/posts 匹配成功
        // Detial.vue 將被渲染到 ProductMenLayout 的 <router-view> 内部，替換card.vue
        path: "detail/:productId",
        component: () => import("../views/product/ProductDetail.vue"),
        meta: {},
        beforeEnter(to, from, next) {
          document.title = `${webTitle}童裝`; //${to.params.productId}
          next();
        },
      },
    ],
  },
  {
    //http://loaclhost/CustomerIndex
    path: "/CustomerIndex",
    component: () => import("../views/CustomeShoes/CustomerIndex.vue"),
    meta: { title: `${webTitle}訂製您的專屬商品` },
  },
  {
    //http://loaclhost/CustomeShoes
    path: "/CustomeShoes",
    component: () => import("../views/CustomeShoes/CustomeShoesLayout.vue"),
    meta: { title: `${webTitle}客製化商品` },
    children: [
      {
        path: "",
        component: () => import("../views/CustomeShoes/CustomeShoesAll.vue"),
      },
      {
        path: ":shoescategoryName",
        component: () => import("../views/CustomeShoes/CustomeShoesAll.vue"),
      },
      {
        //http://loaclhost/Login
        path: "/login",
        component: Login,
      },
    ],
  },
  {
    //http://loaclhost/CustomeShoes/detail/shoesProductId
    path: "/CustomeShoes/detail/:shoesProductId",
    component: () => import("../views/CustomeShoes/ShoesDetail.vue"),
    meta: {},
    beforeEnter(to, from, next) {
      document.title = `${webTitle}商品詳細資訊${to.params.shoesProductId}`;
      next();
    },
  },
  {
    //http://loaclhost/CustomeShoes/detail/Customization/shoesProductId
    path: "/CustomeShoes/detail/Customization/:shoesProductId",
    component: () => import("../views/CustomeShoes/CustomerPage.vue"),
    meta: { require: true },
    beforeEnter(to, from, next) {
      document.title = `${webTitle}商品客製化頁面${to.params.shoesProductId}`;
      next();
    },
  },
  {
    //http://loaclhost/CustomeShoes/detail/Customization/order/ShoesOrderId
    path: "/CustomeShoes/detail/Customization/order/:ShoesOrderId",
    component: () => import("../views/CustomeShoes/ShoesOrderlist.vue"),
    meta: {},
    beforeEnter(to, from, next) {
      document.title = `${webTitle}結帳頁面${to.params.ShoesOrderId}`;
      next();
    },
  },

  {
    //http://loaclhost/CustomeShoes/Contact
    path: "/CustomeShoes/Contact",
    component: () => import("../views/CustomeShoes/Contact.vue"),
    meta: { title: `${webTitle}合作洽詢` },
    children: [
      {
        //http://loaclhost/Login
        path: "/login",
        component: Login,
      },
    ],
  },

  {
    //http://loaclhost/CustomeShoes/Contact
    path: "/CustomeShoes/FAQ",
    component: () => import("../views/CustomeShoes/FAQ.vue"),
    meta: { title: `${webTitle}常見問題` },
    children: [
      {
        //http://loaclhost/Login
        path: "/login",
        component: Login,
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes: routes,
  //  變數名稱跟屬性名稱一樣可直接省略成 routes
});

router.beforeEach((to, from, next) => {
  if (to.meta.title) {
    document.title = to.meta.title;
  }
  const loggedInUser = localStorage.getItem("loggedInUser");
  const beforePath = localStorage.getItem("originalRoute");
  //檢查是否需要驗證，如果需要，則檢查是否已登入
  if (to.meta.require && !loggedInUser) {
    console.log("nologin");
    next({ path: "/login" });
  } else if (to.path == "/login" && loggedInUser) {
    console.log("already logged in");
    if (beforePath) {
      localStorage.removeItem("originalRoute");
      next({ path: beforePath }); // 或其他您希望導向的頁面
    } else {
      next({ path: "/" }); // 或其他您希望導向的頁面
    }
  } else {
    //console.log('login');
    next();
  }
});

export default router;
