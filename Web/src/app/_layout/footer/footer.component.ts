import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
})
export class FooterComponent implements OnInit {
  readonly footerSocial = [
    'fab fa-twitter',
    'fab fa-facebook-f',
    'fab fa-pinterest-p',
  ];
  readonly footerTittle = [
    {
      header: 'Our solutions',
      caption: [
        'Design & creatives',
        'Telecommunication',
        'Restaurant',
        'Programing',
        'Architecture',
      ],
    },
    {
      header: 'Support',
      caption: [
        'Design & creatives',
        'Telecommunication',
        'Restaurant',
        'Programing',
        'Architecture',
      ],
    },
    {
      header: 'Company',
      caption: [
        'Design & creatives',
        'Telecommunication',
        'Restaurant',
        'Programing',
        'Architecture',
      ],
    },
  ];
  constructor() {}

  ngOnInit(): void {}
}
