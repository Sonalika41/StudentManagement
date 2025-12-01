import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [PageHeaderComponent],
  imports: [CommonModule, RouterModule],
  exports: [CommonModule, PageHeaderComponent]
})
export class SharedModule {}
