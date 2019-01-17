import { layoutPaths } from '../../../layout';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'appPicture' })
export class AppPicturePipe implements PipeTransform {

  transform(input: string, ext = 'png'): string {
    return layoutPaths.images.root + input + '.' + ext;
  }
}
