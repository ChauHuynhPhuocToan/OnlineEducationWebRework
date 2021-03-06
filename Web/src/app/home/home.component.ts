import { Component } from '@angular/core';
import { CourseService } from '../services/course.service';
import { Course, CourseDesktopData, CourseDataSet } from '../model/Course';
import { AuthenticationService } from '../services/authentication.service';
import { Account } from '../model/Account';
import { AccountInCourse } from '../model/AccountInventory';
import { AccountincourseService } from '../services/accountincourse.service';
import { CartService } from '../services/cart.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ImageloadService } from '../services/imageload.service';
import { NotificationService } from '../services/notification.service';
import { DatePipe } from '@angular/common';
import { FormControl } from '@angular/forms';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  readonly hastagTitle = [
    'Java',
    'Html/css',
    'Python',
    'IOS/Android',
    'AI',
    'Javascript',
    'Machine Learning',
    'UX/UI',
  ];
  readonly hastagImage = [
    'assets/img/gallery/topic1.png',
    'assets/img/gallery/topic2.png',
    'assets/img/gallery/topic3.png',
    'assets/img/gallery/topic4.png',
    'assets/img/gallery/topic5.png',
    'assets/img/gallery/topic6.png',
    'assets/img/gallery/topic7.png',
    'assets/img/gallery/topic8.png',
  ];
  readonly featuresCaption = [
    'Techniques to engage effectively with vulnerable children and young people.',
    'Join millions of people from around the world learning together.',
    'Join millions of people from around the world learning together. Online learning is as easy and natural.',
  ];
  readonly featuresIcon = [
    'assets/img/icon/icon1.svg',
    'assets/img/icon/icon2.svg',
    'assets/img/icon/icon3.svg',
  ];
  readonly featuresCaptionHead = [
    {
      header: '60+ UX courses',
      caption: 'The automated process all your website tasks.',
    },
    {
      header: 'Expert instructors',
      caption: 'The automated process all your website tasks.',
    },
    {
      header: 'Life time access',
      caption: 'The automated process all your website tasks.',
    },
  ];
  namesearch;
  public courses: Course[] = [];
  public freeCourses: CourseDesktopData[] = [];
  public login = false;
  currentAccount: Account;
  public itembuy: AccountInCourse[];
  // courseDataSet: CourseDataSet[] = [];
  courseDataSet: Course[] = [];
  public freeCourseDataSet: CourseDataSet[] = [];
  isContentToggled: boolean[] = [];
  isContentToggled1: boolean[] = [];
  myControl = new FormControl();
  constructor(
    private router: Router,
    private courseService: CourseService,
    private authenticationService: AuthenticationService,
    private accountincourseService: AccountincourseService,
    private cartService: CartService,
    private route: ActivatedRoute,
    private imageLoadService: ImageloadService,
    private datePipe: DatePipe,
    private notificationService: NotificationService
  ) {
    this.authenticationService.currentAccount.subscribe(
      (x) => (this.currentAccount = x)
    );
    if (this.currentAccount) {
      this.login = true;
    }
    if (this.currentAccount && this.currentAccount.role == 'admin') {
      this.router.navigateByUrl('admin/dashboard');
    }
  }
  async ngOnInit(): Promise<void> {
    await this.load();
  }
  public onSearch = async () => {
    console.log(this.namesearch);
    this.router.navigate(['/search'], {
      queryParams: { name: this.namesearch, option: 0, accountId: 0 },
    });
  };
  public load = async () => {
    this.courses = [];
    this.itembuy = (this.currentAccount) ? await this.accountincourseService.getaccountincoursesByAccountId(this.currentAccount.accountId,1) as AccountInCourse[] : [];
    const list = await this.courseService.gettopcourse() as Course[];
    const list1 = await this.courseService.getTop6CourseBase(1) as CourseDesktopData[];
    const list2 = await this.courseService.getcourses() as Course[];
    var optionCourseDataSet:CourseDataSet[] = [];
    if(this.currentAccount){
      // for(let i = 0; i< list.length;i++){
      //   if(list[i].accountId != this.currentAccount.accountId){
      //     this.courseService.transferToCourseDataset(list[i],this.courseDataSet,this.isContentToggled);
      //   }
      // }
      this.courseDataSet = list;
      console.log(this.courseDataSet[0].renderImagePath);
      console.log(this.courseDataSet[0].renderRating);
      if(list1.length > 0){
        list1.forEach(e => {
          if(e.course.accountId != this.currentAccount.accountId){
            this.courseService.transferToCourseDataset(e.course,this.freeCourseDataSet,this.isContentToggled1);
          }
        })
      }
    }
    else{
      // list.forEach(e =>{
      //   this.courseService.transferToCourseDataset(e,this.courseDataSet,this.isContentToggled);
      // });
      this.courseDataSet = list.map(e => Object.assign(new Course(),e));
      console.log(this.courseDataSet[0].renderImagePath);
      console.log(this.courseDataSet[0].renderRating);
      list1.forEach(e =>{
        this.courseService.transferToCourseDataset(e.course,this.freeCourseDataSet,this.isContentToggled1);
      });
    }
    for(let i = 0; i< list2.length;i++){
        var courseData: CourseDataSet = new CourseDataSet();
        courseData.course = list2[i];
        optionCourseDataSet.push(courseData);
    }
  }
  coursealreadybought(id) {
    if (this.currentAccount == undefined || this.currentAccount == null) {
      return false;
    }
    var find;
    if (this.currentAccount) {
      if (this.itembuy) {
        find = this.itembuy.find((e) => e.courseId == id);
      } else {
        return false;
      }
    }
    return !find ? true : false;
  }
  public loadimage(url) {
    return this.imageLoadService.getImageSource(url);
  }
  addToCart = (course) => {
    if (course.price > 0) {
      this.cartService.addToCart({ course, quantity: 1 });
    } else {
      this.buyFreeCourse(course);
    }
  };
  viewdetail = async (id, course) => {
    await this.courseService.updatecourseviewcount(id, course);
    this.router.navigate(['/category', id], { relativeTo: this.route });
  };
  async buyFreeCourse(course) {
    var invoiceCode = '';
    invoiceCode = await this.accountincourseService.createInvoice(
      this.currentAccount.accountId,
      course.courseId,
      true,
      'Account Balance',
      invoiceCode,
      false,
      this.datePipe.transform(Date.now(), 'yyyy-MM-ddThh:mm:ss')
    );
    await this.accountincourseService.createInvoice(
      course.accountId,
      course.courseId,
      false,
      'Account Balance',
      invoiceCode,
      true,
      this.datePipe.transform(Date.now(), 'yyyy-MM-ddThh:mm:ss')
    );
    this.notificationService.showNotification(
      'This course have been add to your course libary',
      'Buy success',
      1500
    );
    this.router.navigateByUrl('mycourse');
  }
  toggleContent(index) {
    this.courseDataSet[index].renderDescripton = (this.isContentToggled[index]) ? this.courseDataSet[index].description : this.courseDataSet[index].editedDescription;
    // if (type == 'normal') {
    //   this.courseDataSet[index].course.description = this.isContentToggled[
    //     index
    //   ]
    //     ? this.courseDataSet[index].nonFormatedDescription
    //     : this.courseService.formatContent(
    //         this.courseDataSet[index].nonFormatedDescription
    //       );
    // }
    // if (type == 'free') {
    //   this.freeCourseDataSet[index].course.description = this.isContentToggled1[
    //     index
    //   ]
    //     ? this.freeCourseDataSet[index].nonFormatedDescription
    //     : this.courseService.formatContent(
    //         this.freeCourseDataSet[index].nonFormatedDescription
    //       );
    // }
  }
}
